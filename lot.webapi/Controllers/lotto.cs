using LottoLion.BaseLib.Controllers;
using LottoLion.BaseLib.Models.Entity;
using LottoLion.BaseLib.Options;
using LottoLion.BaseLib.Queues;
using LottoLion.BaseLib.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OdinSdk.BaseLib.Configuration;
using OdinSdk.BaseLib.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LottoLion.WebApi.Controllers
{
    [Route("api/[controller]")]
    public partial class LottoController : Controller
    {
        private static CConfig __cconfig = new CConfig();
        private static MemberQ __memberQ;

        private static PrizeReader __prize_reader = new PrizeReader();
        private static TPrizeForcast __prize_forcast = new TPrizeForcast();

        private UserManager __usermgr;
        private LottoLionContext __db_context;

        public LottoController(IOptions<JwtIssuerOptions> jwtOptions, IConfigurationRoot config_root, LottoLionContext db_context)
        {
            __usermgr = new UserManager(jwtOptions.Value);
            __cconfig.SetConfigRoot(config_root);

            __db_context = db_context;
            __memberQ = new MemberQ();
        }

        [Route("GetThisWeekPrize")]
        [Authorize(Policy = "LottoLionUsers")]
        [HttpPost]
        public async Task<IActionResult> GetThisWeekPrize()
        {
            return await CProxy.UsingAsync(async () =>
            {
                var _result = (success: false, message: "ok");

                if (DateTime.Now.Subtract(__prize_forcast.LastReadTime).TotalMinutes > __prize_reader.PrizeReadIntervalMinutes)
                    __prize_forcast = await __prize_reader.ReadPrizeForcast();

                _result.success = true;

                return new OkObjectResult(new
                {
                    success = _result.success,
                    message = _result.message,

                    result = __prize_forcast
                });
            });
        }

        [Route("GetPrizeBySeqNo")]
        [Authorize(Policy = "LottoLionUsers")]
        [HttpPost]
        public async Task<IActionResult> GetPrizeBySeqNo(int sequence_no)
        {
            return await CProxy.Using(() =>
            {
                var _result = (success: false, message: "ok");

                var _winner = (TbLionWinner)null;
                if (sequence_no >= 1 && sequence_no <= WinnerReader.GetThisWeekSequenceNo())
                {
                    _winner = __db_context.TbLionWinner
                                              .Where(w => w.SequenceNo == sequence_no)
                                              .SingleOrDefault();

                    if (_winner == null)
                    {
                        _result.message = $"해당 회차'{sequence_no}'의 추첨 정보가 없습니다";

                        _winner = new TbLionWinner()
                        {
                            SequenceNo = sequence_no,
                            IssueDate = WinnerReader.GetIssueDateBySequenceNo(sequence_no),
                            PaymentDate = WinnerReader.GetPaymentDateBySequenceNo(sequence_no)
                        };
                    }
                    else
                        _result.success = true;
                }
                else
                    _result.message = "추첨 회차가 범위를 벗어 났습니다";

                return new OkObjectResult(new
                {
                    success = _result.success,
                    message = _result.message,

                    result = _winner
                });
            });
        }

        [Route("GetUserChoices")]
        [Authorize(Policy = "LottoLionMember")]
        [HttpPost]
        public async Task<IActionResult> GetUserChoices(int sequence_no)
        {
            return await CProxy.Using(() =>
            {
                var _result = (success: false, message: "ok");

                var _choices = new List<TbLionChoice>();
                {
                    var _login_id = __usermgr.GetLoginId(Request);
                    if (String.IsNullOrEmpty(_login_id) == false)
                    {
                        _choices = __db_context.TbLionChoice
                                            .Where(c => c.LoginId == _login_id && c.SequenceNo == sequence_no)
                                            .OrderByDescending(c => c.Amount)
                                            .ThenBy(c => c.Digit1).ThenBy(c => c.Digit2).ThenBy(c => c.Digit3)
                                            .ThenBy(c => c.Digit4).ThenBy(c => c.Digit5).ThenBy(c => c.Digit6)
                                            .ToList();

                        if (_choices == null)
                            _result.message = $"회원ID '{_login_id}'님의 '{sequence_no}'회차 추출 번호가 없습니다";
                        else
                            _result.success = true;
                    }
                    else
                        _result.message = "인증 정보에서 회원ID를 찾을 수 없습니다";
                }

                return new OkObjectResult(new
                {
                    success = _result.success,
                    message = _result.message,

                    result = _choices
                });
            });
        }

        [Route("GetUserSequenceNos")]
        [Authorize(Policy = "LottoLionMember")]
        [HttpPost]
        public async Task<IActionResult> GetUserSequenceNos()
        {
            return await CProxy.Using(() =>
            {
                var _result = (success: false, message: "ok");

                var _sequence_nos = new List<TKeyValue>();
                {
                    var _login_id = __usermgr.GetLoginId(Request);
                    if (String.IsNullOrEmpty(_login_id) == false)
                    {
                        _sequence_nos = __db_context.TbLionChoice
                                            .Where(c => c.LoginId == _login_id)
                                            .GroupBy(x => x.SequenceNo)
                                            .Select(
                                                y => new TKeyValue
                                                {
                                                    key = y.Key,
                                                    value = y.Count()
                                                }
                                            )
                                            .OrderByDescending(z => z.key)
                                            .ToList();

                        if (_sequence_nos.Count() < 1)
                        {
                            var _today_sequence_no = WinnerReader.GetThisWeekSequenceNo();
                            _sequence_nos.Add(new TKeyValue { key = _today_sequence_no, value = 0 });
                        }

                        _result.success = true;
                    }
                    else
                        _result.message = "인증 정보에서 회원ID를 찾을 수 없습니다";
                }

                return new OkObjectResult(new
                {
                    success = _result.success,
                    message = _result.message,

                    result = _sequence_nos
                });
            });
        }

        [Route("SendChoicedNumbers")]
        [Authorize(Policy = "LottoLionMember")]
        [HttpPost]
        public async Task<IActionResult> SendChoicedNumbers(int sequence_no)
        {
            return await CProxy.UsingAsync(async () =>
            {
                var _result = (success: false, message: "ok");

                var _login_id = __usermgr.GetLoginId(Request);
                if (String.IsNullOrEmpty(_login_id) == false)
                {
                    var _choice = new TChoice()
                    {
                        login_id = _login_id,
                        sequence_no = sequence_no,
                        resend = true
                    };

                    await __memberQ.SendQAsync(_choice);

                    _result.success = true;
                }
                else
                    _result.message = "인증 정보에서 회원ID를 찾을 수 없습니다";

                return new OkObjectResult(new
                {
                    success = _result.success,
                    message = _result.message,

                    result = ""
                });
            });
        }
    }
}
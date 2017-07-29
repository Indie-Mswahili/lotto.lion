using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LottoLion.BaseLib.Types;
using OdinSdk.BaseLib.Configuration;

namespace LottoLion.BaseLib.Controllers
{
    public class PrizeReader
    {
        private static CConfig __cconfig = new CConfig();

        public int PrizeReadIntervalMinutes
        {
            get
            {
                return __cconfig.GetAppInteger("lotto.prize.read.minutes");
            }
        }

        private async Task<HtmlAgilityPack.HtmlDocument> GetHtmlDocument()
        {
            var _site = @"http://www.nlotto.co.kr/common.do?method=main";

            var _result = new HtmlAgilityPack.HtmlDocument
            {
                OptionFixNestedTags = true,
                OptionAutoCloseOnEnd = true
            };

            using (var _client = new HttpClient())
            {
                var _html_bytes = await _client.GetByteArrayAsync(_site);

                var _html_text = Encoding.GetEncoding("euc-kr").GetString(_html_bytes, 0, _html_bytes.Length - 1);
                _result.LoadHtml(_html_text);

                if (_result.DocumentNode == null)
                    throw new Exception($"'{_site}'에서 읽는 중 오류가 발생 하였습니다.");
            }

            return _result;
        }

        private (decimal predict, decimal sales) GetPrizeInfo(HtmlAgilityPack.HtmlDocument html_document)
        {
            var _result = (predict: 0m, sales: 0m);

            var _divisions = html_document.DocumentNode.SelectNodes("//div");
            if (_divisions == null || _divisions.Count < 1)
                throw new Exception($"예상 정보 <div> 추출 중 오류가 발생 하였습니다.");

            var _next_game = _divisions
                                .Where(_d => _d.Attributes.FirstOrDefault(_a => _a.Name == "class" && _a.Value == "clearfx next_info") != null)
                                .FirstOrDefault();

            if (_next_game == null)
                throw new Exception($"예상 정보 <div class=next_game> 추출 중 오류가 발생 하였습니다.");

            var _spans = _next_game.ChildNodes.Where(x => x.Name == "p").ToArray();

            var _predict_amount = _spans[0].ChildNodes[2].InnerText.Split(' ')[1].Replace(",", "");
            _result.predict = Convert.ToDecimal(_predict_amount);

            var _sales_amount = _spans[1].ChildNodes[2].InnerText.Split('\t')[8].Split('\r', '\n')[0].Replace(",", "");
            _result.sales = Convert.ToDecimal(_sales_amount);

            return _result;
        }

        public async Task<TPrizeForcast> ReadPrizeForcast()
        {
            var _now_time = DateTime.Now;

            var _resut = new TPrizeForcast()
            {
                PredictAmount = 0,
                SalesAmount = 0,

                SequenceNo = WinnerReader.GetNextWeekSequenceNo(),
                IssueDate = WinnerReader.GetNextWeekIssueDate(),
                
                LastReadTime = _now_time,
                NextReadTime = _now_time.AddMinutes(PrizeReadIntervalMinutes),
                ReadInterval = PrizeReadIntervalMinutes
            };

            try
            {
                var _html_document = await GetHtmlDocument();

                var _prize_info = GetPrizeInfo(_html_document);
                {
                    _resut.PredictAmount = _prize_info.predict;
                    _resut.SalesAmount = _prize_info.sales;
                }
            }
            catch (Exception)
            {
            }

            return _resut;
        }
    }
}
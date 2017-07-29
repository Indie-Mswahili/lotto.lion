using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using LottoLion.BaseLib.Models;
using LottoLion.BaseLib.Types;
using OdinSdk.BaseLib.Configuration;
using LottoLion.BaseLib.Models.Entity;

namespace LottoLion.BaseLib.Controllers
{
    public class PrintOutLottoLion
    {
        private static CConfig __cconfig = new CConfig();

        //-------------------------------------------------------------------------------------------------------------
        // The A4 size print measures 21.0 x 29.7cm, 8.27 x 11.69 inches, if mounted 30.3 x 40.6cm, 11.93 x 15.98 inches.
        //-------------------------------------------------------------------------------------------------------------

        private const float __page_width = 8.27F * 96F;
        private const float __page_height = 11.69F * 96F;

        private const float __left_margin = 0F;
        private const float __top_margin = 10F;

        private const float __slip_width = 194F;
        private const float __slip_height = 82.5F;

        private const float __y_gap = 6.4F;
        private const float __x_gap = 3.43F;

        private const int __max_slip_per_page = 3;
        private const int __max_number_per_slip = 5;

        private Image __black_marker = null;
        private Image BlackMarker
        {
            get
            {
                if (__black_marker == null)
                    __black_marker = Image.FromFile(Path.GetFullPath(@"images/black_marker.bmp"));
                return __black_marker;
            }
        }

        private Image __slip_paper = null;
        private Image SlipPaper
        {
            get
            {
                if (__slip_paper == null)
                    __slip_paper = Image.FromFile(Path.GetFullPath(@"images/lotto_slip_096_1529_0649.jpg"));
                return __slip_paper;
            }
        }

        private IEnumerable<Bitmap> PrintLottoSheet(TbLionChoice[] lott645_selector, int sequence_no, string login_id)
        {
            var _row_number = 0;

            do
            {
                var _destination = new Bitmap((int)__page_width, (int)__page_height);

                var _lotto_sheet = Graphics.FromImage(_destination);
                {
                    _lotto_sheet.PageUnit = GraphicsUnit.Millimeter;
                    _lotto_sheet.Clear(Color.White);
                }

                var _top_margin = __top_margin;

                for (int i = 0; i < __max_slip_per_page; i++)
                {
                    var _rectangle = new RectangleF(__left_margin, _top_margin, __slip_width, __slip_height);
                    _lotto_sheet.DrawImage(SlipPaper, _rectangle);

                    // boxing
                    var _pen = new Pen(Color.Gray, 0.1f);
                    {
                        _pen.DashStyle = DashStyle.DashDotDot;
                        _pen.DashCap = DashCap.Round;
                        _pen.DashPattern = new float[] { 5.0f, 15.0f };

                        _lotto_sheet.SmoothingMode = SmoothingMode.AntiAlias;
                        _lotto_sheet.DrawRectangle(_pen, __left_margin, _top_margin, __slip_width, __slip_height);
                    }

                    for (int j = 0; j < __max_number_per_slip; j++)
                    {
                        var _s_row = lott645_selector[_row_number];
                        var _s_number = new short[]
                        {
                            _s_row.Digit1,
                            _s_row.Digit2,
                            _s_row.Digit3,
                            _s_row.Digit4,
                            _s_row.Digit5,
                            _s_row.Digit6
                        };

                        for (int k = 0; k < 6; k++)
                        {
                            var _number = _s_number[k];

                            var _y_position = 11.0F + __top_margin + i * __slip_height;
                            var _x_position = 44.0F + __left_margin + j * 27.4F;

                            var _y_offset = (_number - 1) / 7;
                            var _x_offset = (_number - _y_offset * 7) - 1;

                            _y_position += __y_gap * _y_offset;
                            _x_position += __x_gap * _x_offset;

                            _rectangle = new RectangleF(_x_position, _y_position, 2.0F, 3.5F);
                            _lotto_sheet.DrawImage(BlackMarker, _rectangle);
                        }

                        if (++_row_number >= lott645_selector.Length)
                        {
                            i = __max_slip_per_page;
                            break;
                        }
                    }

                    _top_margin += __slip_height;
                }

                yield return _destination;
            }
            while (_row_number < lott645_selector.Length);
        }

        private string ServiceName
        {
            get
            {
                return __cconfig.GetAppString("lotto.sender.service.name");
            }
        }

        private string __zip_folder = null;
        private string ZipFolder
        {
            get
            {
                if (__zip_folder == null)
                    __zip_folder = Path.Combine(Path.GetTempPath(), ServiceName);
                return __zip_folder;
            }
        }

        public string SaveLottoSheet(TbLionChoice[] lott645_selector, TChoice choice)
        {
            // location of files to compress
            var _zip_directory = "";
            {
                var _zip_packing = $"{ServiceName}_{choice.login_id}_{choice.sequence_no:0000}";

                var _user_directory = Path.Combine(ZipFolder, choice.login_id);
                {
                    if (Directory.Exists(_user_directory) == true)
                        Directory.Delete(_user_directory, true);

                    Directory.CreateDirectory(_user_directory);
                }

                _zip_directory = Path.Combine(_user_directory, _zip_packing);
                {
                    if (Directory.Exists(_zip_directory) == true)
                        Directory.Delete(_zip_directory, true);

                    Directory.CreateDirectory(_zip_directory);
                }
            }

            // write sheet images
            var _page_no = 1;
            {
                foreach (var _sheet in PrintLottoSheet(lott645_selector, choice.sequence_no, choice.login_id))
                {
                    var _file_name = $"{ServiceName}_{choice.login_id}_{choice.sequence_no:0000}_{_page_no:000}.jpg";
                    {
                        var _img_filepath = Path.Combine(_zip_directory, _file_name);
                        if (File.Exists(_img_filepath))
                            File.Delete(_img_filepath);

                        _sheet.Save(_img_filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    _page_no++;
                }
            }

            // write text file
            {
                var _file_name = $"{ServiceName}_{choice.login_id}_{choice.sequence_no:0000}.csv";

                var _txt_filepath = Path.Combine(_zip_directory, _file_name);
                if (File.Exists(_txt_filepath))
                    File.Delete(_txt_filepath);

                using (var _writer = File.OpenWrite(_txt_filepath))
                {
                    using (var _stream = new StreamWriter(_writer))
                    {
                        foreach (var _s in lott645_selector)
                        {
                            var _line = $"{_s.SequenceNo},,"
                                      + $"{_s.Digit1},{_s.Digit2},{_s.Digit3},{_s.Digit4},{_s.Digit5},{_s.Digit6}";

                            _stream.WriteLine(_line);
                        }
                    }
                }
            }

            // create zip file
            var _zip_file = Path.ChangeExtension(_zip_directory, "zip");
            {
                if (File.Exists(_zip_file))
                    File.Delete(_zip_file);

                ZipFile.CreateFromDirectory(_zip_directory, _zip_file);
            }

            return _zip_file;
        }
    }
}
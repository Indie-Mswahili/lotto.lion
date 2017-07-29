using Foundation;
using Google.MobileAds;
using Lion.XiOS.Libs;
using Lion.XIOS.Type;
using System;
using System.Collections;
using UIKit;

namespace Lion.XiOS
{
    public partial class PrizeViewController : UIViewController, IUITableViewDelegate, IUITableViewDataSource, IBannerViewDelegate, IVideoControllerDelegate
    {
        public bool adReceived;

        private NSNumberFormatter numberFormatter;
        private int tempNum;
        private int lastSeqNum;
        private ArrayList seqArray;
        private ArrayList amountArray;
        private ArrayList countArray;
        private NextWeekPrize ThisWeekPrizeDic;
        private WinnerPrize PrizeBySeqDic;

        static string AdUnitId = "ca-app-pub-8599301686845489/3287348059";

        public PrizeViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            adReceived = true;
            SeqView.Hidden = true;
            internetView.Hidden = true;

            numberFormatter = new NSNumberFormatter();
            numberFormatter.Locale = NSLocale.CurrentLocale;
            numberFormatter.NumberStyle = NSNumberFormatterStyle.Decimal;

            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            if (String.IsNullOrEmpty(appDelegate.GuestToken) == true)
            {
                this.GetGuestTokenForPrizeView();
                this.GetThisWeekPrizeDic();
            }

            if (appDelegate.RecentSeqNum > 0)
            {
                this.LabelsSetting();
                lastSeqNum = appDelegate.RecentSeqNum - 1;
                tempNum = lastSeqNum;
                this.GetPrizeBySeqDic(lastSeqNum);
                prizeTableView.ReloadData();
                this.ConfigureView();
                this.MakeSeqCount();
            }
        }
        public override void ViewWillAppear(bool animated)
        {
            this.InternetCheck();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.GetThisWeekPrizeDic();
            this.LabelsSetting();

            if (tempNum != lastSeqNum)
            {
                this.myLoadIndicator.StartAnimating();
                tempNum = lastSeqNum;
                this.GetPrizeBySeqDic(lastSeqNum);
                prizeTableView.ReloadData();
                this.ConfigureView();
                this.myLoadIndicator.StopAnimating();
            }
        }

        private void GetPrizePerform()
        {
            this.GetPrizeBySeqDic(lastSeqNum);

            prizeTableView.ReloadData();
            this.myLoadIndicator.StopAnimating();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        private void LabelsSetting()
        {
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

            lblSeq.Text = $"{appDelegate.RecentSeqNum}회차";
            lblDate.Text = $"{TimeFormatter.GetStringFromDatetime(ThisWeekPrizeDic.IssueDate)}";
            lblPrize.Text = $"{ThisWeekPrizeDic.PredictAmount:#,##0}원";
            lblPrize2.Text = $"이번주 누적 판매금액 : {ThisWeekPrizeDic.SalesAmount:#,##0}원";
        }

        private void GetGuestTokenForPrizeView()
        {
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;

            try
            {
                appDelegate.GuestToken = appDelegate.NetworkInstance.GetGuestToken().result;
            }
            catch (Exception ex)
            {
                ConfigHelper.ShowConfirm("오류", ex.Message, "확인");
            }
        }

        private void ConfigureView()
        {
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            lblSeq.Text = $"{appDelegate.RecentSeqNum}회차";
            lblSeq2.Text = String.Format("{0}회차", tempNum);
            lblDate.Text = $"{TimeFormatter.GetStringFromDatetime(ThisWeekPrizeDic.IssueDate)}";
            lblDate2.Text = $"{TimeFormatter.GetStringFromDatetime(PrizeBySeqDic.issueDate)}";
            lblPrize.Text = $"{ThisWeekPrizeDic.PredictAmount:#,##0}원";
            lblPrize2.Text = $"이번주 누적 판매금액 : {ThisWeekPrizeDic.SalesAmount:#,##0}원";

            lblNum1.Text = PrizeBySeqDic.digit1.ToString();
            lblNum2.Text = PrizeBySeqDic.digit2.ToString();
            lblNum3.Text = PrizeBySeqDic.digit3.ToString();
            lblNum4.Text = PrizeBySeqDic.digit4.ToString();
            lblNum5.Text = PrizeBySeqDic.digit5.ToString();
            lblNum6.Text = PrizeBySeqDic.digit6.ToString();
            lblNum7.Text = PrizeBySeqDic.digit7.ToString();

            imgNum1.Image = new UIImage(this.MakeImgName(PrizeBySeqDic.digit1));
            imgNum2.Image = new UIImage(this.MakeImgName(PrizeBySeqDic.digit2));
            imgNum3.Image = new UIImage(this.MakeImgName(PrizeBySeqDic.digit3));
            imgNum4.Image = new UIImage(this.MakeImgName(PrizeBySeqDic.digit4));
            imgNum5.Image = new UIImage(this.MakeImgName(PrizeBySeqDic.digit5));
            imgNum6.Image = new UIImage(this.MakeImgName(PrizeBySeqDic.digit6));
            imgNum7.Image = new UIImage(this.MakeImgName(PrizeBySeqDic.digit7));

            amountArray = new ArrayList {
                PrizeBySeqDic.amount1,
                PrizeBySeqDic.amount2,
                PrizeBySeqDic.amount3,
                PrizeBySeqDic.amount4,
                PrizeBySeqDic.amount5
            };

            countArray = new ArrayList {
                PrizeBySeqDic.count1,
                PrizeBySeqDic.count2,
                PrizeBySeqDic.count3,
                PrizeBySeqDic.count4,
                PrizeBySeqDic.count5
            };
        }

        private string MakeImgName(int number)
        {
            var imgName = "Circled-Y.png";
            if (number < 11)
            {
                imgName = "Circled-Y.png";
            }
            else if (number < 21)
            {
                imgName = "Circled-B.png";
            }
            else if (number < 31)
            {
                imgName = "Circled-R.png";
            }
            else if (number < 41)
            {
                imgName = "Circled-K.png";
            }
            else if (number < 46)
            {
                imgName = "Circled-G.png";
            }

            return imgName;
        }

        private void MakeSeqCount()
        {
            seqArray = new ArrayList();
            for (int i = 0; i < 100; i++)
                seqArray.Add(String.Format("{0}", lastSeqNum - i));
        }

        private void GetThisWeekPrizeDic()
        {
            try
            {
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                if (String.IsNullOrEmpty(appDelegate.GuestToken) == false)
                {
                    var responseDict = appDelegate.NetworkInstance.GetThisWeekPrize(appDelegate.GuestToken);
                    ThisWeekPrizeDic = responseDict.result;

                    appDelegate.RecentSeqNum = ThisWeekPrizeDic.SequenceNo;
                    appDelegate.RecentSeqDate = $"{TimeFormatter.GetStringFromDatetime(ThisWeekPrizeDic.IssueDate)}";
                }
            }
            catch (Exception ex)
            {
                ConfigHelper.ShowConfirm("오류", ex.Message, "확인");
            }

            this.myLoadIndicator.StopAnimating();
        }

        private void GetPrizeBySeqDic(int sequence_no)
        {
            try
            {
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                if (String.IsNullOrEmpty(appDelegate.GuestToken) == false)
                {
                    PrizeBySeqDic = appDelegate.NetworkInstance.GetPrizeBySeqNo(appDelegate.GuestToken, sequence_no).result;

                    this.myLoadIndicator.StartAnimating();

                    prizeTableView.PerformSelector(new ObjCRuntime.Selector("reloadData"), myLoadIndicator, 0);

                    this.myLoadIndicator.StopAnimating();
                }
            }
            catch (Exception ex)
            {
                ConfigHelper.ShowConfirm("오류", ex.Message, "확인");
            }
        }

        partial void BtnTurnAction(UIButton sender)
        {
            SeqView.Hidden = false;
        }

        partial void BtnCloseAction(UIButton sender)
        {
            SeqView.Hidden = true;
        }

        partial void BtnOkAction(UIButton sender)
        {
            SeqView.Hidden = true;
        }

        [Export("tableView:willDisplayCell:forRowAtIndexPath:")]
        public void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            if (tableView.RespondsToSelector(new ObjCRuntime.Selector("SeparatorInset")))
            {
                tableView.SeparatorInset = UIEdgeInsets.Zero;
            }

            if (tableView.RespondsToSelector(new ObjCRuntime.Selector("LayoutMargins")))
            {
                tableView.LayoutMargins = UIEdgeInsets.Zero;
            }

            if (cell.RespondsToSelector(new ObjCRuntime.Selector("LayoutMargins")))
            {
                cell.LayoutMargins = UIEdgeInsets.Zero;
            }

        }

        [Export("tableView:viewForHeaderInSection:")]
        public UIView GetViewForHeader(UITableView tableView, int section)
        {
            if (tableView.Tag == 11)
            {
                var header = NSBundle.MainBundle.LoadNib("AdsViewCell", this, null).GetItem<AdsViewCell>(0);
                header.GADBannerView.AdUnitID = AdUnitId;
                header.GADBannerView.Delegate = this;
                header.GADBannerView.RootViewController = this;

                var request = Request.GetDefaultRequest();

                if (ConfigHelper.IsSimulator)
                    request.TestDevices = new string[] { "kGADSimulatorID" }; // GADRequest.GAD_SIMULATOR_ID 

                header.GADBannerView.LoadRequest(request);
                return header;
            }
            else
            {
                return null;
            }
        }

        [Export("tableView:heightForHeaderInSection:")]
        public Single GetHeightForHeader(UITableView tableView, int section)
        {
            var _result = (Single)0;

            if (tableView.Tag == 11)
            {
                if (adReceived)
                    _result = 51.0f;
            }

            return _result;
        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            return (tableView.Tag == 11) ? 5 : 55;
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Tag == 11)
            {
                var simpleTableIdentifier = "PrizeCell";

                var cell = tableView.DequeueReusableCell(simpleTableIdentifier) as PrizeCell;
                if (cell == null)
                {
                    NSArray nib = NSBundle.MainBundle.LoadNib("PrizeCell", this, null);
                    cell = nib.GetItem<PrizeCell>(0);
                }

                cell.Label1.Text = String.Format("{0}등", (long)indexPath.Row + 1);
                cell.Label2.Text = String.Format("{0}명", numberFormatter.StringFromNumber(new NSNumber(Convert.ToInt32(countArray[indexPath.Row].ToString()))));
                cell.Label3.Text = String.Format("{0}원", numberFormatter.StringFromNumber(new NSNumber(Convert.ToInt64(amountArray[indexPath.Row].ToString()))));

                var bColor = UIColor.White;
                if (indexPath.Row % 2 != 0)
                    bColor = new UIColor(210, 210, 210, 0.6f);

                cell.BackgroundColor = bColor;
                return cell;
            }
            else
            {
                var CellIdentifier = "SeqCell";

                var cell = tableView.DequeueReusableCell(CellIdentifier);
                if (cell == null)
                    cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);

                cell.Accessory = UITableViewCellAccessory.None;
                cell.TextLabel.Text = String.Format("{0}회", seqArray[indexPath.Row]);

                cell.TextLabel.TextAlignment = UITextAlignment.Center;
                return cell;
            }
        }

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Tag == 99)
            {
                tempNum = Convert.ToInt32(seqArray[indexPath.Row]);

                this.myLoadIndicator.StartAnimating();

                this.GetPrizeBySeqDic(tempNum);
                this.ConfigureView();

                SeqView.Hidden = true;
                tableView.DeselectRow(indexPath, true);
            }
        }

        private void InternetCheck()
        {
        }

        [Export("adViewDidReceiveAd:")]
        public void DidReceiveAd(NativeExpressAdView nativeExpressAdView)
        {
            if (nativeExpressAdView.VideoController.HasVideoContent())
            {
            }
            else
            {
            }
        }

        [Export("videoControllerDidEndVideoPlayback:")]
        public void DidEndVideoPlayback(VideoController videoController)
        {
        }

        [Export("adView:didFailToReceiveAdWithError:")]
        public void DidFailToReceiveAd(BannerView adView, RequestError error)
        {
        }
    }
}
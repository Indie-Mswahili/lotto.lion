using Foundation;
using Google.MobileAds;
using Lion.XiOS.Libs;
using Lion.XIOS.Type;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Lion.XiOS
{
    public partial class NumbersViewController : UIViewController, IUITableViewDelegate, IUITableViewDataSource, IBannerViewDelegate, IVideoControllerDelegate, IUITextFieldDelegate
    {
        public List<UserChoice> NumbersArray { get; set; }
        public int RecentSeqNum { get; set; }
        public string RecentSeqDate { get; set; }
        //public string UserToken { get; set; }
        public bool adReceived;

        NSNumberFormatter numberFormatter;
        int presentSeqNum;
        int lastSeqNum;

        ArrayList seqArray;
        int firstSeq;
        static string AdUnitId = "ca-app-pub-8599301686845489/3287348059";

        public NumbersViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            seqArray = new ArrayList();
            adReceived = true;
            this.loginView.Hidden = false;
            numberFormatter = new NSNumberFormatter();
            numberFormatter.Locale = NSLocale.CurrentLocale;
            numberFormatter.NumberStyle = NSNumberFormatterStyle.Decimal;
            this.seqView.Hidden = true;
        }
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewWillAppear(bool animated)
        {
            this.InternetCheck();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.loginView.Hidden = false;
            this.CheckLoginAndMove();
        }

        private void CheckLoginAndMove()
        {
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            if (String.IsNullOrEmpty(appDelegate.UserToken) == true || appDelegate.Logined == false)
            {
                var viewCont = this.Storyboard.InstantiateViewController("loginView") as LoginViewController;
                this.NavigationController.PushViewController(viewCont, false);
            }
            else
            {
                this.myIndicator.StartAnimating();

                presentSeqNum = 0;
                RecentSeqNum = appDelegate.RecentSeqNum;
                RecentSeqDate = appDelegate.RecentSeqDate;

                this.PerformSelector(new ObjCRuntime.Selector("SettingViews"), this.myIndicator, 0);
            }
        }

        [Export("SettingViews")]
        private void SettingViews()
        {
            if (presentSeqNum == 0)
            {
                presentSeqNum = RecentSeqNum;
                lastSeqNum = RecentSeqNum - 1;

                this.GetNumbers(RecentSeqNum);
                this.GetUserLastSeqNo();
            }

            if (presentSeqNum == RecentSeqNum)
            {
                this.btnForward.Hidden = true;
                this.lblTurn.Text = String.Format("{0}회차 (예상)", RecentSeqNum);
            }
            else if (firstSeq == RecentSeqNum)
            {
                this.btnForward.Hidden = true;
                this.btnBackward.Hidden = true;
                this.lblTurn.Text = String.Format("{0}회차 (예상)", RecentSeqNum);
            }
            else
            {
                this.btnForward.Hidden = false;
                this.lblTurn.Text = String.Format("{0}회차 (예상)", RecentSeqNum);
            }

            this.myIndicator.StopAnimating();
        }

        string MakeImgName(int number)
        {
            var _result = "Circled-Y.png";
            if (number < 11)
            {
                _result = "Circled-Y.png";
            }
            else if (number < 21)
            {
                _result = "Circled-B.png";
            }
            else if (number < 31)
            {
                _result = "Circled-R.png";
            }
            else if (number < 41)
            {
                _result = "Circled-K.png";
            }
            else if (number < 46)
            {
                _result = "Circled-G.png";
            }

            return _result;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            this.View.EndEditing(true);
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            return true;
        }

        [Export("textFieldDidEndEditing:")]
        public void EditingEnded(UITextField textField)
        {
            textField.ResignFirstResponder();
        }

        private void GetUserInfo()
        {
            try
            {
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                if (String.IsNullOrEmpty(appDelegate.UserToken) == false)
                {
                    var responseDict = appDelegate.NetworkInstance.GetUserInfor(appDelegate.UserToken);
                    appDelegate.UserInfo = responseDict.result;
                }
            }
            catch (Exception)
            {
                this.ExcuteLogout();
            }
        }

        private void GetNumbers(int sequence_no)
        {
            try
            {
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                if (String.IsNullOrEmpty(appDelegate.UserToken) == false)
                {
                    if (RecentSeqNum >= 0)
                    {
                        var responseDict = appDelegate.NetworkInstance.GetUserChoices(appDelegate.UserToken, sequence_no);
                        NumbersArray = responseDict.result;

                        this.numbersTable.PerformSelector(new ObjCRuntime.Selector("reloadData"), this.myIndicator, 0);
                        if (NumbersArray.Count == 0 && RecentSeqNum == sequence_no)
                        {
                            this.loginView.Hidden = false;
                            this.noticeLabel.Text = "나의 예상 번호를 생성하고 있습니다.\n예상 소요시간은 30분 입니다.";
                        }
                        else if (NumbersArray.Count == 0)
                        {
                            this.loginView.Hidden = false;
                            this.noticeLabel.Text = "당첨된 번호가 없습니다.";
                        }
                        else
                        {
                            this.loginView.Hidden = true;
                            this.noticeLabel.Text = "Loading ...";
                        }
                    }
                }
                else
                    this.ExcuteLogout();
            }
            catch (Exception)
            {
                this.ExcuteLogout();
            }
        }

        private void GetUserLastSeqNo()
        {
            try
            {
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                if (String.IsNullOrEmpty(appDelegate.UserToken) == false)
                {
                    var responseDict = appDelegate.NetworkInstance.GetUserSequenceNos(appDelegate.UserToken);
                    firstSeq = responseDict.result.FirstOrDefault().key;
                }
                else
                    this.ExcuteLogout();
            }
            catch (Exception)
            {
                this.ExcuteLogout();
            }
        }

        private void BtnSignUpAction(UIButton sender)
        {
            var viewCont = this.Storyboard.InstantiateViewController("signUpview") as SignUpViewController;
            this.NavigationController.PushViewController(viewCont, true);
        }

        partial void BtnTurnAction(UIButton sender)
        {
            this.seqView.Hidden = false;
        }

        partial void BtnBackAction(UIButton sender)
        {
            this.seqView.Hidden = true;
        }

        partial void BtnTurnOkAction(UIButton sender)
        {
            this.seqView.Hidden = true;
        }

        partial void BtnMessageAction(UIButton sender)
        {
            var _message = $"{RecentSeqNum}회차 예상 번호를 메일로 다시 받으시겠습니까?\n재발송에 1분 정도 소요됩니다.";

            ConfigHelper.ShowYesNo("메일 재발송", _message, "재발송", "취소", action =>
            {
                this.ResendUserNumbers(RecentSeqNum);
            });
        }

        partial void SeqforwardAction(UIButton sender)
        {
            this.myIndicator.StartAnimating();
            presentSeqNum++;

            this.btnBackward.Hidden = false;
            if (presentSeqNum == RecentSeqNum)
            {
                this.btnForward.Hidden = true;
                this.lblTurn.Text = String.Format("{0}회차 (예상)", presentSeqNum);
            }
            else
            {
                this.btnForward.Hidden = false;
                this.lblTurn.Text = String.Format("{0}회차 (완료)", presentSeqNum);
            }

            this.PerformSelector(new ObjCRuntime.Selector("LoadingUserNumbers"), this.myIndicator, 0);
        }

        partial void SeqBackAction(UIButton sender)
        {
            this.myIndicator.StartAnimating();
            presentSeqNum--;

            this.btnForward.Hidden = false;
            if (presentSeqNum == 0 || presentSeqNum == firstSeq)
            {
                this.btnBackward.Hidden = true;
            }

            this.lblTurn.Text = String.Format("{0}회차 (완료)", presentSeqNum);
            this.PerformSelector(new ObjCRuntime.Selector("LoadingUserNumbers"), this.myIndicator, 0);
        }

        [Export("LoadingUserNumbers")]
        public void LoadingUserNumbers()
        {
            this.GetNumbers(presentSeqNum);
            this.myIndicator.StopAnimating();
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
            if (tableView.Tag == 111)
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
            if (tableView.Tag == 111)
            {
                if (adReceived)
                {
                    return (Single) 51.0;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }

        public nint RowsInSection(UITableView tableView, nint section)
        {
            if (tableView.Tag == 111)
            {
                var _cnt = 0;
                if (NumbersArray != null)
                    _cnt = Convert.ToInt32(NumbersArray.Count.ToString());
                return _cnt;
            }
            else
            {
                return 55;
            }
        }

        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (tableView.Tag == 111)
            {
                var simpleTableIdentifier = "NumbersCell";

                var cell = tableView.DequeueReusableCell(simpleTableIdentifier) as NumbersCell;
                if (cell == null)
                {
                    NSArray nib = NSBundle.MainBundle.LoadNib("NumbersCell", this, null);
                    cell = nib.GetItem<NumbersCell>(0);
                }

                cell.WhiteView.Hidden = true;
                cell.Num1.Text = String.Format("{0}", NumbersArray[indexPath.Row].digit1);
                cell.Num2.Text = String.Format("{0}", NumbersArray[indexPath.Row].digit2);
                cell.Num3.Text = String.Format("{0}", NumbersArray[indexPath.Row].digit3);
                cell.Num4.Text = String.Format("{0}", NumbersArray[indexPath.Row].digit4);
                cell.Num5.Text = String.Format("{0}", NumbersArray[indexPath.Row].digit5);
                cell.Num6.Text = String.Format("{0}", NumbersArray[indexPath.Row].digit6);
                cell.ImgNum1.Image = new UIImage(this.MakeImgName(NumbersArray[indexPath.Row].digit1));
                cell.ImgNum2.Image = new UIImage(this.MakeImgName(NumbersArray[indexPath.Row].digit2));
                cell.ImgNum3.Image = new UIImage(this.MakeImgName(NumbersArray[indexPath.Row].digit3));
                cell.ImgNum4.Image = new UIImage(this.MakeImgName(NumbersArray[indexPath.Row].digit4));
                cell.ImgNum5.Image = new UIImage(this.MakeImgName(NumbersArray[indexPath.Row].digit5));
                cell.ImgNum6.Image = new UIImage(this.MakeImgName(NumbersArray[indexPath.Row].digit6));
                if (NumbersArray[indexPath.Row].ranking == 6)
                {
                    cell.WhiteView.Hidden = false;
                    cell.Label1.Text = "당첨 결과 : X";
                    cell.Label2.Text = String.Format("당첨금 : {0}원", numberFormatter.StringFromNumber(new NSNumber((float)NumbersArray[indexPath.Row].amount)));
                }
                else if (NumbersArray[indexPath.Row].ranking == 0)
                {
                    cell.Label1.Text = String.Format("추첨 일자 : {0}", RecentSeqDate);
                    cell.Label2.Text = "1등 당첨을 기원합니다";
                }
                else
                {
                    cell.Label1.Text = String.Format("당첨 결과 : {0}등", numberFormatter.StringFromNumber(new NSNumber(NumbersArray[indexPath.Row].ranking)));
    
                    cell.Label2.Text = String.Format("당첨금 : {0}원", numberFormatter.StringFromNumber(new NSNumber((float)NumbersArray[indexPath.Row].amount)));
                }

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
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                }

                cell.Accessory = UITableViewCellAccessory.None;
                cell.TextLabel.Text = String.Format("{0}회", seqArray[indexPath.Row]);
                if (indexPath.Row == 0)
                {
                    cell.TextLabel.Text = String.Format("{0}회 (예상번호)", seqArray[indexPath.Row]);
                }

                cell.TextLabel.TextAlignment = UITextAlignment.Center;
                return cell;
            }
        }

        private void ResendUserNumbers(int seqNo)
        {
            try
            {
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                if (String.IsNullOrEmpty(appDelegate.UserToken) == false)
                {
                    var responseDict = appDelegate.NetworkInstance.SendChoicedNumbers(appDelegate.UserToken, seqNo);
                    if (responseDict.success == true)
                        ConfigHelper.ShowConfirm("알림", "가입하신 이메일주소로 예상번호를 발송했습니다.", "확인");
                    else
                        ConfigHelper.ShowConfirm("알림", responseDict.message, "확인");
                }
                else
                    this.ExcuteLogout();
            }
            catch (Exception)
            {
                this.ExcuteLogout();
            }
        }

        private void ExcuteLogout()
        {
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            {
                appDelegate.Logined = false;
                appDelegate.UserToken = "";
                appDelegate.UserInfo = null;
            }

            //UserToken = "";

            var _viewCont2 = this.Storyboard.InstantiateViewController("loginView") as LoginViewController;
            this.NavigationController.PushViewController(_viewCont2, true);

            ConfigHelper.ShowConfirm("알림", "네트워크 연결 불안정으로 로그아웃 되었습니다.\n연결상태 확인 후 앱을 재실행 해주세요.", "확인");
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
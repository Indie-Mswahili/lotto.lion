using Foundation;
using Lion.XiOS.Libs;
using System;
using UIKit;

namespace Lion.XiOS
{
    public partial class LoginViewController : UITableViewController, IUITextFieldDelegate
    {
        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.loginButtonCell.TextLabel.BackgroundColor = UIColor.Clear;
            this.signUpButtonCell.TextLabel.BackgroundColor = UIColor.Clear;
            this.renewButtonCell.TextLabel.BackgroundColor = UIColor.Clear;

            this.NavigationItem.Title = "로그인";
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            this.InternetCheck();

            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            if (appDelegate.Logined)
            {
                this.NavigationController.PopViewController(true);
            }
            else
            {
                if (String.IsNullOrEmpty(appDelegate.GuestToken) == true)
                    this.GetGuestToken();
                else
                    NSLogHelper.NSLog("Guest Token Stored");

                this.SetSwitchs();
            }
        }

        private void SetSwitchs()
        {
            if (NSUserDefaults.StandardUserDefaults.BoolForKey("isSaveID"))
                this.switchID.On = true;
            else
                this.switchID.On = false;

            if (NSUserDefaults.StandardUserDefaults.BoolForKey("isSavePW"))
                this.switchPW.On = true;
            else
                this.switchPW.On = false;

            if (String.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey("ID")) == false)
                this.tfID.Text = NSUserDefaults.StandardUserDefaults.StringForKey("ID");

            if (String.IsNullOrEmpty(NSUserDefaults.StandardUserDefaults.StringForKey("PW")) == false)
                this.tfPW.Text = NSUserDefaults.StandardUserDefaults.StringForKey("PW");
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            this.TableView.EndEditing(true);
        }

        [Export("textFieldShouldReturn:")]
        public bool ShouldReturn(UITextField textField)
        {
            if (textField.Tag == 1)
            {
                this.tfPW.BecomeFirstResponder();
            }
            else
            {
                textField.ResignFirstResponder();
            }

            return true;
        }

        [Export("textFieldDidEndEditing:")]
        public void EditingEnded(UITextField textField)
        {
            textField.ResignFirstResponder();
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 3;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            if (section == 0)
            {
                return 3;
            }
            else if (section == 1)
            {
                return 1;
            }
            else if (section == 2)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            if (indexPath.Section == 0)
            {
                if (indexPath.Row == 2)
                    this.LoginWithIDPassword(this.tfID.Text, this.tfPW.Text);
            }
            else if (indexPath.Section == 1 && indexPath.Row == 0)
            {
                if (appDelegate.Logined)
                {
                    ConfigHelper.ShowConfirm("알림", "로그인 상태입니다.", "확인");
                    return;
                }
                else
                {
                    var signUpViewCont = this.Storyboard.InstantiateViewController("signUpview") as SignUpViewController;
                    this.NavigationController.PushViewController(signUpViewCont, true);
                }
            }
            else if (indexPath.Section == 2 && indexPath.Row == 0)
            {
                var renewViewCont = this.Storyboard.InstantiateViewController("renewView") as RenewViewController;
                this.NavigationController.PushViewController(renewViewCont, true);
            }

            tableView.DeselectRow(indexPath, true);
        }

        private void GetGuestToken()
        {
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            appDelegate.GuestToken = appDelegate.NetworkInstance.GetGuestToken().result;
        }

        private void LoginWithIDPassword(string login_id, string password)
        {
            try
            {
                var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
                if (String.IsNullOrEmpty(appDelegate.GuestToken) == false)
                {
                    var responseDict = appDelegate.NetworkInstance.GetTokenByLoginId(appDelegate.GuestToken, login_id, password, "I", appDelegate.APNSKey);

                    if (responseDict.success == true)
                    {
                        var _user_token = responseDict.result;
                        if (String.IsNullOrEmpty(_user_token) == false)
                        {
                            appDelegate.UserToken = _user_token;

                            if (this.switchID.On)
                                NSUserDefaults.StandardUserDefaults.SetString(this.tfID.Text, "ID");
                            else
                                NSUserDefaults.StandardUserDefaults.SetString("", "ID");

                            if (this.switchPW.On)
                                NSUserDefaults.StandardUserDefaults.SetString(this.tfPW.Text, "PW");
                            else
                                NSUserDefaults.StandardUserDefaults.SetString("", "PW");

                            this.GetUserInfo(appDelegate.UserToken);

                            appDelegate.Logined = true;
                            appDelegate.Edited = true;

                            this.NavigationController.PopViewController(true);
                        }
                    }
                    else
                        ConfigHelper.ShowConfirm("오류", responseDict.message, "확인");
                }
            }
            catch (Exception ex)
            {
                ConfigHelper.ShowConfirm("오류", ex.Message, "확인");
            }
        }

        private void GetUserInfo(string user_token)
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
            catch (Exception ex)
            {
                ConfigHelper.ShowConfirm("오류", ex.Message, "확인");
            }
        }

        partial void IDSwitchAction(UISwitch sender)
        {
            if (sender.On)
            {
                NSUserDefaults.StandardUserDefaults.SetBool(true, "isSaveID");
            }
            else
            {
                NSUserDefaults.StandardUserDefaults.SetBool(false, "isSaveID");
            }
        }

        partial void PWSwitchAction(UISwitch sender)
        {
            if (sender.On)
                NSUserDefaults.StandardUserDefaults.SetBool(true, "isSavePW");
            else
                NSUserDefaults.StandardUserDefaults.SetBool(false, "isSavePW");
        }

        private void InternetCheck()
        {
        }
    }
}
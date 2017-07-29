using Foundation;
using Google.MobileAds;
using Lion.XiOS.Libs;
using Lion.XIOS.Type;
using System.Text;
using UIKit;
using UserNotifications;

namespace Lion.XiOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window
        {
            get;
            set;
        }

        public string UserToken { get; set; }

        public string GuestToken { get; set; }

        public int RecentSeqNum { get; set; }

        public string RecentSeqDate { get; set; }

        public UserInfo UserInfo { get; set; }

        public bool Edited { get; set; }

        public bool Logined { get; set; }

        public Networking NetworkInstance { get; set; }

        public string UserID { get; set; }

        public string UserPW { get; set; }

        public string APNSKey { get; set; }

        public AppDelegate() : base()
        {
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            this.UserSetting();
            this.NetworkInstance = new Networking();
            this.RegisterForRemoteNotifications();

            MobileAds.Configure("ca-app-pub-8599301686845489~1509563653");

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.

            this.Logined = false;
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.

            this.Logined = false;
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            this.GuestToken = this.NetworkInstance.GetGuestToken().result;
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            NSLogHelper.NSLog($"Failed to get token, error: {error}");
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var deviceId = new StringBuilder();

            var ptr = deviceToken.ToByteArray();
            for (int i = 0; i < 32; i++)
            {
                deviceId.AppendFormat("%x02", ptr[i]);
            }

            this.APNSKey = deviceId.ToString();
        }

        private void RegisterForRemoteNotifications()
        {
            // version 10.0 upper
            if (UIDevice.CurrentDevice.SystemVersion.CompareTo("10.0") >= 0)
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Sound | UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge,
                    (approved, error) =>
                    {
                        if (error != null)
                        {
                            UIApplication.SharedApplication.RegisterForRemoteNotifications();
                        }
                    });
                UNUserNotificationCenter.Current.Delegate = new MyUNUserNotificationCenterDelegate();
            }
            else
            {
                UIApplication.SharedApplication.RegisterUserNotificationSettings(
                        UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Sound | UIUserNotificationType.Alert | UIUserNotificationType.Badge, null)
                    );
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }

            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        private void UserSetting()
        {
            var dicOptionDefaults = NSDictionary.FromObjectsAndKeys(
                    new object[] {
                        "", "", false, false
                    },
                    new object[] {
                        "ID", "PW", "isSaveID", "isSavePW"
                    }
                );
            NSUserDefaults.StandardUserDefaults.RegisterDefaults(dicOptionDefaults);

            this.Edited = false;
            this.Logined = false;
            this.UserInfo = new UserInfo();

            NSUserDefaults.StandardUserDefaults.SetBool(true, "isSaveID");
            NSUserDefaults.StandardUserDefaults.SetBool(true, "isSavePW");
        }
    }
}
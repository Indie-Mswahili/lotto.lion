using Android.App;
using Android.Content;
using Android.Runtime;
using Firebase.Messaging;
using Lion.XDroid.Libs;

namespace Lion.XDroid.Service
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class PushReceiverService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage remoteMessage)
        {
            var data = remoteMessage.Data;

            var notifyBuilder = (new Notification.Builder(this)) 
                                    .SetContentTitle(data["title"])
                                    .SetContentText(data["message"])
                                    .SetSmallIcon(Resource.Drawable.alarm)
                                    .SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis()
                                 );

            var _notification_manager = this.ApplicationContext.GetSystemService(Context.NotificationService).JavaCast<NotificationManager>();
            _notification_manager.Notify(0, notifyBuilder.Build());

            AppCommon.SNG.ResetBadge(this);
        }
    }
}
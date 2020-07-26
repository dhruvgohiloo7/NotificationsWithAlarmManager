using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;


namespace NotificationsWithAlarmManager.Droid
{
    [BroadcastReceiver(Enabled = true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            string Title = intent.GetStringExtra("Title");
            string Message = intent.GetStringExtra("Message");
            int NotificationId = intent.GetIntExtra("NotificationId", 0);
            string time = intent.GetStringExtra("Time");
            NotificationInterval interval = (NotificationInterval)Enum.Parse(typeof(NotificationInterval), intent.GetStringExtra("Interval"));

            var notificationDate = DateTime.Parse(time);
            if (interval == NotificationInterval.None && notificationDate.Date != DateTime.Now.Date)
                return;

            SendNotification(context, Title, Message);
        }

        void SendNotification(Context context, string title, string message)
        {
            Random rm = new Random();
            int NotificationId = rm.Next(9999);
            var channelId = "MyApp Channel";
            var mainIntent = new Intent(context, typeof(MainActivity));
            mainIntent.PutExtra("NotificationId", NotificationId);

            mainIntent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(context, NotificationId, mainIntent, PendingIntentFlags.UpdateCurrent);


            var style = new NotificationCompat.BigTextStyle();
            style.BigText(title);

            var defaultSoundUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
            var notificationBuilder = new NotificationCompat.Builder(context, channelId)
                .SetContentTitle("MyApp - " + title)
                .SetContentText(message)
                .SetAutoCancel(true)
                .SetSound(defaultSoundUri)
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(Resource.Drawable.icon);

            var notificationManager = NotificationManager.FromContext(context);

            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.O)
            {

                NotificationChannel mChannel = new NotificationChannel(channelId, "MyApp", NotificationImportance.High);
                notificationManager.CreateNotificationChannel(mChannel);
                notificationBuilder.SetChannelId(channelId);
            }

            notificationManager.Notify(NotificationId, notificationBuilder.Build());
        }
    }
}
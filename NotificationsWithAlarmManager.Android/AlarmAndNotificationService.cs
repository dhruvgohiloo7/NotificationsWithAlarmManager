using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NotificationsWithAlarmManager.Droid;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using static Android.App.AlarmManager;

[assembly: Dependency(typeof(AlarmAndNotificationService))]
namespace NotificationsWithAlarmManager.Droid
{
    public class AlarmAndNotificationService : IAlarmAndNotificationService
    {
        void IAlarmAndNotificationService.ScheduleLocalNotification(string notificationTitle, string notificationMessage, DateTime specificDateTime, TimeSpan timeSpan, int notificationId, NotificationInterval interval)
        {
            DateTime utcDateTime = new DateTime(specificDateTime.Ticks + timeSpan.Ticks).ToUniversalTime();
            Java.Util.Date nativeDate = DateTimeToNativeDate(utcDateTime);

            Intent alarmReciver = new Intent(Forms.Context, typeof(AlarmReceiver));
            alarmReciver.PutExtra("Title", notificationTitle);
            alarmReciver.PutExtra("Message", notificationMessage);
            alarmReciver.PutExtra("NotificationId", notificationId);
            alarmReciver.PutExtra("Time", (specificDateTime + timeSpan).ToString());
            alarmReciver.PutExtra("Interval", interval.ToString());


            System.Diagnostics.Debug.WriteLine($"Schedule LocalNotification : for time : {nativeDate.ToString()}");

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, notificationId, alarmReciver, PendingIntentFlags.CancelCurrent);
            AlarmClockInfo alarmClockInfo = new AlarmClockInfo(nativeDate.Time, pendingIntent);

            var alarmManager = (AlarmManager)Android.App.Application.Context.GetSystemService(Context.AlarmService);

            if (interval == NotificationInterval.Day)
                alarmManager.SetRepeating(AlarmType.RtcWakeup, nativeDate.Time, IntervalDay, pendingIntent);
            else if (interval == NotificationInterval.Hours)
                alarmManager.SetRepeating(AlarmType.RtcWakeup, nativeDate.Time, IntervalHour, pendingIntent);
            else if (interval == NotificationInterval.Week)
                alarmManager.SetRepeating(AlarmType.RtcWakeup, nativeDate.Time, IntervalDay * 7, pendingIntent);
            else if (interval == NotificationInterval.None)
                alarmManager.Set(AlarmType.RtcWakeup, nativeDate.Time, pendingIntent);
        }

        public static Java.Util.Date DateTimeToNativeDate(DateTime date)
        {
            long dateTimeUtcAsMilliseconds = (long)date.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalMilliseconds;
            return new Java.Util.Date(dateTimeUtcAsMilliseconds);
        }

        void IAlarmAndNotificationService.CancelNotification(int notificationId)
        {
            Intent alarmReciver = new Intent(Forms.Context, typeof(AlarmReceiver));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(Forms.Context, notificationId, alarmReciver, PendingIntentFlags.CancelCurrent);

            if (pendingIntent != null)
            {
                var alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
                alarmManager.Cancel(pendingIntent);
            }
        }
    }
}
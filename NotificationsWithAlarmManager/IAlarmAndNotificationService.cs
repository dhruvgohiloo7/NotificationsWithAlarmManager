using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationsWithAlarmManager
{
    public interface IAlarmAndNotificationService
    {
        void ScheduleLocalNotification(string notificationTitle, string notificationMessage, DateTime specificDateTime, TimeSpan timeSpan, int notificationId, NotificationInterval interval);

        void CancelNotification(int notificationId);
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NotificationsWithAlarmManager
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly IAlarmAndNotificationService _platformNotificationService;
        public MainPage()
        {
            InitializeComponent();
            _platformNotificationService = DependencyService.Get<IAlarmAndNotificationService>();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            CancelNotifications();
            int notificationId = 700;
            var StartDate = new DateTime(2020, 08, 01);
            for (int i = 0; i < 10; i++)
            {
                notificationId++;
                var NotificationDate = StartDate.AddDays(i);

                _platformNotificationService.ScheduleLocalNotification("Local Notification", "Hi, I am from local notification", NotificationDate.Date, new TimeSpan(15, 0, 0), notificationId, NotificationInterval.None);
            }
        }

        void CancelNotifications()
        {
            int notificationId = 700;
            for (int i = 0; i < 10; i++)
            {
                notificationId++;
                _platformNotificationService.CancelNotification(notificationId);
            }
        }
    }
}

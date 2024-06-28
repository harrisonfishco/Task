namespace Task.Notifications
{
    public class NotificationService
    {
        public readonly INotification Notification;
        public NotificationService() 
        {
            Notification = new EmailNotification();
        }
    }
}

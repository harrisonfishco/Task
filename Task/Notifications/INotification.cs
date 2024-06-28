namespace Task.Notifications
{
    public interface INotification
    {
        public void Send(TaskUser to, string subject, string message, NotificationContentType type = NotificationContentType.Info);

        public void SendMass(List<TaskUser> to, string subject, string message, NotificationContentType type = NotificationContentType.Info)
        {
            to.ForEach(t =>
            {
                Send(t, subject, message, type);
            });
        }
    }

    public enum NotificationContentType
    {
        Info,
    }
}

using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Task.Notifications
{
    public class EmailNotification : INotification
    {
        public void Send(TaskUser to, string subject, string message, NotificationContentType type = NotificationContentType.Info)
        {
            //TODO: using parameters (not hardcoding) and take from the saved settings
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Vanessa Hegmann", "vanessa.hegmann@ethereal.email"));
            email.To.Add(new MailboxAddress(to.FullName, to.Email));
            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("vanessa.hegmann@ethereal.email", "HT3Z5UhKDcUvw9Y5tf");

                client.Send(email);
                client.Disconnect(true);
            }
        }
    }
}

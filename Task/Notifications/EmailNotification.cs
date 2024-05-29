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
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Vanessa Hegmann", "vanessa.hegmann@ethereal.email"));
            email.To.Add(new MailboxAddress("Vanessa Hegmann", "vanessa.hegmann@ethereal.email"));
            email.Subject = "TESTING EMAIL WITH ETHEREAL";

            email.Body = new TextPart("plain")
            {
                Text = @"Hey Chandler,

                I just wanted to let you know that Monica and I were going to go play some paintball, you in?

                -- Joey"
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

        //from mailbox name
        //from email
        //smtp server
        //smtp username(email addy)
        //smtp passwrod(your password)
        //smtp port (default = 587)

        //test button (sending to current user)
        //save button (enable if valid request was sent)

        //store in a file (next to the exe the working path)

        //test with smtp.dreamhost.com
    }
}

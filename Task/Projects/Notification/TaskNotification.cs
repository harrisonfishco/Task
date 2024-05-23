using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Notification
{
    public class TaskNotification
    {
        public static void main(string[] args)
        {
            SendEmail();
        }
        public static void SendEmail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Vanessa Hegmann", "vanessa.hegmann@ethereal.email"));
            message.To.Add(new MailboxAddress("Vanessa Hegmann", "vanessa.hegmann@ethereal.email"));
            message.Subject = "TESTING EMAIL WITH ETHEREAL";

            message.Body = new TextPart("plain")
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

                client.Send(message);
                client.Disconnect(true);
            }
        }

    }
}

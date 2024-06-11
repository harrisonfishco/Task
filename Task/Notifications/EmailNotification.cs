using System;
using System.Net.Sockets;
using System.Threading;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Task.Components.Pages;

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

        public bool IsSent(TaskUser to, SmtpSettings smtp)
        {
            bool connection = false;
            try
            {
                /*if (smtp == null)
                {
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress(smtp.FromMailboxName, smtp.FromEmail));
                    email.To.Add(new MailboxAddress(smtp.FromMailboxName, smtp.FromEmail));
                    email.Subject = @"Test Subject";

                    email.Body = new TextPart("plain")
                    {
                        Text = @"Test Body"
                    };

                    using (var client = new SmtpClient())
                    {
                        client.Connect(smtp.SmtpServer, smtp.SmtpPort, SecureSocketOptions.StartTls);

                        // Note: only needed if the SMTP server requires authentication
                        client.Authenticate(smtp.SmtpUsername, smtp.SmtpPassword);

                        client.Send(email);
                        client.Disconnect(true);
                    }
                    return true;
                }
                return false; */

                //using (var tcpClient = new TcpClient())
                //var connectTask = tcpClient.ConnectAsync("smtp.ethereal.email", 587);
                TcpClient smtpTest = new TcpClient();
                smtpTest.Connect(smtp.SmtpServer, smtp.SmtpPort);
                if (smtpTest.Connected)
                {
                    NetworkStream ns = smtpTest.GetStream();
                    StreamReader sr = new StreamReader(ns);
                    if (sr.ReadLine().Contains("220"))
                    {
                        connection = true;
                        System.Diagnostics.Debug.Print("220 received returning true");
                    }
                    smtpTest.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print($"Exception occurred: {ex.Message}");
            }
            return connection;
        }
    }
}

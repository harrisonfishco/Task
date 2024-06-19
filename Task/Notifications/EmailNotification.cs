using System.Net.Sockets;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Task.Components.Pages;
using System.Text.Json;
using Task;

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

                client.Authenticate("vanessa.hegmann@ethereal.email", "HT3Z5UhKDcUvw9Y5tf");

                client.Send(email);
                client.Disconnect(true);
            }
        }

        public int SendTestEmail(TaskUser to, SmtpSettings smtp)
        {
            Random random = new Random();
            int fourDigitCode = random.Next(1000, 10000);

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(smtp.FromMailboxName, smtp.FromEmail));
            email.To.Add(new MailboxAddress(to.FullName, smtp.FromEmail));
            email.Subject = "Test Email";

            //TODO: Set up four digit code
            email.Body = new TextPart("plain")
            {
                Text = $"Code: {fourDigitCode}"
            };

            using (var client = new SmtpClient())
            {
                client.Connect(smtp.SmtpServer, smtp.SmtpPort, SecureSocketOptions.StartTls);

                client.Authenticate(smtp.SmtpUsername, smtp.SmtpPassword);

                client.Send(email);
                client.Disconnect(true);
            }
            return fourDigitCode;
        }

        public bool IsSent(TaskUser to, SmtpSettings smtp)
        {
            bool connection = false;
            try
            {
                SmtpClient smtpTest = new SmtpClient();
                smtpTest.Connect(smtp.SmtpServer, smtp.SmtpPort);
                smtpTest.Authenticate(smtp.SmtpUsername, smtp.SmtpPassword);
                if (smtpTest.IsAuthenticated)
                {
                    connection = true;
                    System.Diagnostics.Debug.Print("Connected!");
                    smtpTest.Disconnect(true);
                }
            }
            //TODO: Set up error messages for user
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.HostNotFound)
                {
                    System.Diagnostics.Debug.Print("Error: No such host is known. Please check the server address.");
                }
                else if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    System.Diagnostics.Debug.Print("Error: Please check the port number.");
                }
            }
            catch (AuthenticationException ex)
            {
                System.Diagnostics.Debug.Print("Error: Invalid email or password");
            }
            return connection;
        }

        //TODO: Create the json with code

        public async Task<SmtpSettings> ReadSmtpSettings()
        {
            //if(TaskCache.ContainsKey())
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"bin\Debug\net8.0", "smtpSettings.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} was not found.");
            }

            var json = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<SmtpSettings>(json);
        }

        public async Task<bool> WriteSmtpSettings(SmtpSettings settings)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"bin\Debug\net8.0", "smtpSettings.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} was not found.");
            }

            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
            return true;
        }
    }
}

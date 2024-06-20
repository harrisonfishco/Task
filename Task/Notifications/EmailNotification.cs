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
        public readonly string TASK_SMTP_SETTINGS_KEY = "TASK_SMTP_SETTINGS_KEY";
        public async void Send(TaskUser to, string subject, string message, NotificationContentType type = NotificationContentType.Info)
        {
            SmtpSettings smtpSettings = new SmtpSettings();
            string json;
            if (TaskCache.ContainsKey(TASK_SMTP_SETTINGS_KEY))
            {
                json = TaskCache.GetKey(TASK_SMTP_SETTINGS_KEY)!;
                smtpSettings = JsonSerializer.Deserialize<SmtpSettings>(json)!;
            }
            else
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"bin\Debug\net8.0", "smtpSettings.json");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"The file {filePath} was not found.");
                }

                json = await File.ReadAllTextAsync(filePath);

                TaskCache.SetKey(TASK_SMTP_SETTINGS_KEY, json);
            }

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(smtpSettings.FromMailboxName, smtpSettings.FromEmail));
            email.To.Add(new MailboxAddress(to.FullName, to.Email));
            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect(smtpSettings.SmtpServer, smtpSettings.SmtpPort, SecureSocketOptions.StartTls);

                client.Authenticate(smtpSettings.SmtpUsername, smtpSettings.SmtpPassword);

                client.Send(email);
                client.Disconnect(true);
            }
        }

        public void SendTestEmail(TaskUser to, SmtpSettings smtp)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(smtp.FromMailboxName, smtp.FromEmail));
            email.To.Add(new MailboxAddress(to.FullName, smtp.FromEmail));
            email.Subject = "Test Email";

            email.Body = new TextPart("plain")
            {
                Text = "This is a test Email"
            };

            using (var client = new SmtpClient())
            {
                client.Connect(smtp.SmtpServer, smtp.SmtpPort, SecureSocketOptions.StartTls);

                client.Authenticate(smtp.SmtpUsername, smtp.SmtpPassword);

                client.Send(email);
                client.Disconnect(true);
            }
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
                    //TaskError.CreateUserError("Connected!");
                    smtpTest.Disconnect(true);
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.HostNotFound)
                {
                    TaskError.CreateUserError("Error: No such host is known. Please check the server address.");
                }
                else if (ex.SocketErrorCode == SocketError.ConnectionRefused)
                {
                    TaskError.CreateUserError("Error: Please check the port number.");
                }
            }
            catch (AuthenticationException ex)
            {
                TaskError.CreateUserError("Error: Invalid email or password");
            }

            SendTestEmail(to, smtp);

            return connection;
        }

        //TODO: Create the json with code

        public async Task<SmtpSettings> ReadSmtpSettings()
        {
            string json;

            if(TaskCache.ContainsKey(TASK_SMTP_SETTINGS_KEY))
            {
                json = TaskCache.GetKey(TASK_SMTP_SETTINGS_KEY)!;
            }
            else
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"bin\Debug\net8.0", "smtpSettings.json");

                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"The file {filePath} was not found.");
                }

                json = await File.ReadAllTextAsync(filePath);

                TaskCache.SetKey(TASK_SMTP_SETTINGS_KEY, json);
            }

            SmtpSettings? res = JsonSerializer.Deserialize<SmtpSettings>(json);
            if(TypeCheck.Empty(res))
            {
                TaskError.CreateUserError("Smtp Settings Missing!");
            }
            return res!;
        }

        public async Task<bool> WriteSmtpSettings(SmtpSettings settings)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"bin\Debug\net8.0", "smtpSettings.json");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} was not found.");
            }

            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

            TaskCache.SetKey(TASK_SMTP_SETTINGS_KEY, json);

            await File.WriteAllTextAsync(filePath, json);
            return true;
        }
    }
}

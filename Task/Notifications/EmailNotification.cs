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

        private string FilePath
        {
            get
            {
                string res = string.Empty;
                try
                {
                    res = Path.Combine(Directory.GetCurrentDirectory(), "smtpSettings.json");
#if DEBUG
                    res = Path.Combine(Directory.GetCurrentDirectory(), @"bin/Debug/net8.0", "smtpSettings.json");
#endif
                } catch(Exception ex) { TaskError.HandleError(ex); }
                return res;
            }
        }

        public async void Send(TaskUser to, string subject, string message, NotificationContentType type = NotificationContentType.Info)
        {
            try
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
                    if (!File.Exists(FilePath))
                    {
                        CreateDefaultFile();
                    }

                    json = await File.ReadAllTextAsync(FilePath);

                    TaskCache.SetKey(TASK_SMTP_SETTINGS_KEY, json);
                }

                MimeMessage email = new MimeMessage();
                email.From.Add(new MailboxAddress(smtpSettings.FromMailboxName, smtpSettings.FromEmail));
                email.To.Add(new MailboxAddress(to.FullName, to.Email));
                email.Subject = subject;

                email.Body = new TextPart("plain")
                {
                    Text = message
                };

                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(smtpSettings.SmtpServer, smtpSettings.SmtpPort, SecureSocketOptions.StartTls);

                    client.Authenticate(smtpSettings.SmtpUsername, smtpSettings.SmtpPassword);

                    client.Send(email);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                TaskError.CreateUserError(ex.Message);
            }
            
        }

        public void SendTestEmail(TaskUser to, SmtpSettings smtp)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(new MailboxAddress(smtp.FromMailboxName, smtp.FromEmail));
            email.To.Add(new MailboxAddress(to.FullName, to.Email));
            email.Subject = "Test Email";

            email.Body = new TextPart("plain")
            {
                Text = "This is a test Email"
            };

            using (SmtpClient client = new SmtpClient())
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
                    smtpTest.Disconnect(true);
                    SendTestEmail(to, smtp);
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
            catch(Exception ex) { TaskError.HandleError(ex); }

            return connection;
        }

        public async Task<SmtpSettings> ReadSmtpSettings()
        {
            string json;
            SmtpSettings ? res = null;

            try
            {
                if (TaskCache.ContainsKey(TASK_SMTP_SETTINGS_KEY))
                {
                    json = TaskCache.GetKey(TASK_SMTP_SETTINGS_KEY)!;
                }
                else
                {
                    if (!File.Exists(FilePath))
                    {
                        CreateDefaultFile();
                    }

                    json = await File.ReadAllTextAsync(FilePath);

                    TaskCache.SetKey(TASK_SMTP_SETTINGS_KEY, json);
                }

                res = JsonSerializer.Deserialize<SmtpSettings>(json);
                if (TypeCheck.Empty(res))
                {
                    TaskError.CreateUserError("Smtp Settings Missing!");
                }
            }
            catch (Exception ex)
            {
                TaskError.HandleError(ex);
            }

            return res!;
        }

        public async void WriteSmtpSettings(SmtpSettings settings)
        {
            try
            {
                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

                TaskCache.SetKey(TASK_SMTP_SETTINGS_KEY, json);

                await File.WriteAllTextAsync(FilePath, json);
            } 
            catch(Exception ex) 
            { 
                TaskError.HandleError(ex); 
            }
        }

        private async void CreateDefaultFile(SmtpSettings? settings = null)
        {
            try
            {
                if (TypeCheck.Empty(settings))
                {
                    settings = new SmtpSettings();
                }

                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(FilePath, json);
            }
            catch (Exception ex)
            {
                TaskError.HandleError(ex);
            }
        }
    }
}

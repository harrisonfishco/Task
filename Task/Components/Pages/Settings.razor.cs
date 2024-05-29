namespace Task.Components.Pages
{
    public partial class Settings
    {
    }

    internal class SmtpSettings
    {
        public string FromMailboxName { get; set; } = string.Empty;
        public string FromEmail {  get; set; } = string.Empty;
        public string SmtpServer { get; set; } = string.Empty;
        public string SmtpUsername {  get; set; } = string.Empty;
        public string SmtpPassword { get; set;} = string.Empty;
        public short SmtpPort { get; set; } = 587;
    }
}

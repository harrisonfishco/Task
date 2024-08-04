using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Task.Components.Pages
{
    public partial class Settings
    {
        
    }

    //create and instance, check if file exists, create file, write to file
    public class SmtpSettings
    {
        public string FromMailboxName { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string SmtpServer { get; set; } = string.Empty;
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public short SmtpPort { get; set; } = 587;

    }

    public class LdapSettings
    {
        public const int LDAP_DEFAULT_PORT = 389;
        public const int LDAPS_DEFAULT_PORT = 636;

        public bool UseLdap { get; set; } = false;
        public bool UseLdaps { get; set; } = false;
        public string LdapServer { get; set; } = string.Empty;
        public short LdapPort { get; set; } = LDAP_DEFAULT_PORT;
        public string SearchBase { get; set; } = string.Empty;
        public string SearchFilter { get; set; } = string.Empty;
        public string QueryUser { get; set; } = string.Empty;
        public string QueryPassword { get; set; } = string.Empty;
        public string UserFilter { get; set; } = string.Empty;
        public string UsernameAttribuate { get; set; } = "userPrincipalName";
        public string MailAttribuate { get; set; } = "mail";
        public string Domain
        {
            get
            {
                return $"{(UseLdaps ? "ldaps" : "ldap")}://{LdapServer}:{LdapPort}";
            }
        }

    } 
}

namespace AspireTesting.AppHost.Integrations.MailHog;

public class SmtpConfig
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = MailHogResource.DefaultSmtpPort;
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool UseSsl { get; set; }
    public string FromAddress { get; set; } = MailHogResource.DefaultAddressFrom;
}

namespace AspireTesting.AppHost.Integrations.MailHog;

public class MailHogResource : ContainerResource, IResourceWithServiceDiscovery
{
    internal SmtpConfig SmtpConfig { get; }

    public MailHogResource(string name) : base(name)
    {
        SmtpConfig = new();
    }
}

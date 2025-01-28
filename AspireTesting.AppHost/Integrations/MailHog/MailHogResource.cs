using System.Text.Json;

namespace AspireTesting.AppHost.Integrations.MailHog;

public class MailHogResource : ContainerResource, IResourceWithConnectionString
{
    internal const int DefaultHttpPort = 8025;
    internal const int DefaultSmtpPort = 1025;

    internal const string SmtpEndpointName = "smtp";
    internal const string HttpEndpointName = "http";

    internal const string DefaultAddressFrom = "smtp@example.com";

    internal SmtpConfig SmtpConfig { get; }

    private EndpointReference? _smtpReference;

    private EndpointReference SmtpEndpoint =>
        _smtpReference ??= new EndpointReference(this, SmtpEndpointName);

    public MailHogResource(string name) : base(name)
    {
        SmtpConfig = new();
    }

    public ReferenceExpression ConnectionStringExpression
    {
        get
        {
            SmtpConfig.Host = SmtpEndpoint.Host;
            SmtpConfig.Port = SmtpEndpoint.Port;
            return ReferenceExpression.Create($"{JsonSerializer.Serialize(SmtpConfig)}");
        }
    }

}

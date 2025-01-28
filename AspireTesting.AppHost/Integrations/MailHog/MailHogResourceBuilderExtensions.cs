namespace AspireTesting.AppHost.Integrations.MailHog;

public static class MailHogResourceBuilderExtensions
{
    public static IResourceBuilder<MailHogResource> AddMailHog(
        this IDistributedApplicationBuilder builder,
        string name)
    {
        var resource = new MailHogResource(name);

        var mailhog = builder.AddResource(resource)
            .WithImage("mailhog/mailhog");

        return mailhog
            .WithPorts();
    }

    public static IResourceBuilder<MailHogResource> WithPorts(
        this IResourceBuilder<MailHogResource> builder,
        int httpPort = MailHogResource.DefaultHttpPort,
        int smtpPort = MailHogResource.DefaultSmtpPort)
    {
        builder
            .WithEndpoint(targetPort: MailHogResource.DefaultHttpPort, port: httpPort, scheme: MailHogResource.HttpEndpointName)
            .WithEndpoint(targetPort: MailHogResource.DefaultSmtpPort, port: smtpPort, scheme: MailHogResource.SmtpEndpointName);

        return builder;
    }

    public static IResourceBuilder<MailHogResource> FromAddress(
        this IResourceBuilder<MailHogResource> builder,
        string address = MailHogResource.DefaultAddressFrom)
    {
        builder.Resource.SmtpConfig.FromAddress = address;
        return builder;
    }

    public static IResourceBuilder<MailHogResource> UseSsl(
        this IResourceBuilder<MailHogResource> builder)
    {
        builder.Resource.SmtpConfig.UseSsl = true;
        return builder;
    }
}

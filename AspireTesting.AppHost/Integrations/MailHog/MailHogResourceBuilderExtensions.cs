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
            .WithPorts()
            .FromAddress(MailHogResource.DefaultAddressFrom);
    }

    public static IResourceBuilder<MailHogResource> WithPorts(
        this IResourceBuilder<MailHogResource> builder,
        int httpPort = MailHogResource.DefaultHttpPort,
        int smtpPort = MailHogResource.DefaultSmtpPort)
    {
        builder.Resource.TryGetAnnotationsOfType<EndpointAnnotation>(out var endpoints);

        if (endpoints != null && endpoints.Any())
        {
            foreach (var endpoint in endpoints)
            {
                builder.Resource.Annotations.Remove(endpoint);
            }
        }

        builder
            .WithEndpoint(targetPort: MailHogResource.DefaultHttpPort, 
                port: httpPort, 
                scheme: MailHogResource.HttpEndpointName,
                name: MailHogResource.HttpEndpointName)
            .WithEndpoint(targetPort: MailHogResource.DefaultSmtpPort, 
                port: smtpPort, 
                scheme: MailHogResource.SmtpEndpointName,
                name: MailHogResource.SmtpEndpointName);

        return builder;
    }

    public static IResourceBuilder<MailHogResource> FromAddress(
        this IResourceBuilder<MailHogResource> builder,
        string address)
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

    public static IResourceBuilder<TDestination> WithReference<TDestination>(
        this IResourceBuilder<TDestination> builder, IResourceBuilder<MailHogResource> source)
        where TDestination : IResourceWithEnvironment
    {
        var endpoint = source.GetEndpoint(MailHogResource.SmtpEndpointName);

        // Enter the context to access endpoint.
        builder.WithEnvironment(context =>
        {
            source.Resource.SmtpConfig.Host = endpoint.Host;
            source.Resource.SmtpConfig.Port = endpoint.Port;
    
            var smtpConfigType = source.Resource.SmtpConfig.GetType();

            foreach (var property in smtpConfigType.GetProperties())
            {
                var value = property.GetValue(source.Resource.SmtpConfig);
            
                var key = $"{source.Resource.Name}__{property.Name}";
            
                context.EnvironmentVariables.Add(key, value?.ToString() ?? string.Empty);
            }
        });

        return builder;
    }
}

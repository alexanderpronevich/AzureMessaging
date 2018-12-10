using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqBindings.RabbitMqTrigger;

namespace RabbitMqBindings
{
    public static class RabbitMqWebJobBuilderExtensions
    {
        public static IWebJobsBuilder AddRabbitMq(this IWebJobsBuilder builder)
        {
            builder.AddExtension<RabbitMqTriggerExtensionConfigProvider>()
                .Services
                .AddSingleton<ConsumerRabbitMqChannelFactory>()
                .AddSingleton<PublisherRabbitMqChannelFactory>();
            return builder;
        }
    }
}

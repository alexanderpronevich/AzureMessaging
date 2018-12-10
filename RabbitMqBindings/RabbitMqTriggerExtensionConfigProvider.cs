using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using RabbitMqBindings.RabbitMqMessage;
using RabbitMqBindings.RabbitMqTrigger;

namespace RabbitMqBindings
{
    internal class RabbitMqTriggerExtensionConfigProvider: IExtensionConfigProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ConsumerRabbitMqChannelFactory _channelFactory;
        private readonly PublisherRabbitMqChannelFactory _syncChannelFactory;

        public RabbitMqTriggerExtensionConfigProvider(IConfiguration configuration, ConsumerRabbitMqChannelFactory channelFactory, PublisherRabbitMqChannelFactory syncChannelFactory)
        {
            _configuration = configuration;
            _channelFactory = channelFactory;
            _syncChannelFactory = syncChannelFactory;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var triggerRule = context.AddBindingRule<RabbitMqTriggerAttribute>();
            triggerRule.BindToTrigger<string>(new RabbitMqTriggerAttributeBindingProvider(_configuration, _channelFactory));

            var outputRule = context.AddBindingRule<RabbitMqMessageAttribute>();
            outputRule.BindToCollector(BuildAsyncCollector);
        }

        private IAsyncCollector<string> BuildAsyncCollector(RabbitMqMessageAttribute attribute)
        {
            var connection = _configuration.GetConnectionStringOrSetting(attribute.ConnectionStringSetting);
            var channel = _syncChannelFactory.CreateChannel(connection);

            return new RabbitMqCollector(channel, attribute.ExchangeName, attribute.RoutingKey, attribute.Mandatory, attribute.DeliveryMode);
        }
    }
}
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Extensions.Configuration;

namespace RabbitMqBindings.RabbitMqTrigger
{
    internal class RabbitMqTriggerAttributeBindingProvider : ITriggerBindingProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ConsumerRabbitMqChannelFactory _channelFactory;

        public RabbitMqTriggerAttributeBindingProvider(IConfiguration configuration,
            ConsumerRabbitMqChannelFactory channelFactory)
        {
            _configuration = configuration;
            _channelFactory = channelFactory;
        }

        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            var parameter = context.Parameter;
            var attribute = parameter.GetCustomAttribute<RabbitMqTriggerAttribute>(inherit: false);
            if (attribute == null)
            {
                return Task.FromResult<ITriggerBinding>(null);
            }

            var connectionString = _configuration.GetConnectionStringOrSetting(attribute.ConnectionStringSetting);
            var channel = _channelFactory.CreateChannel(connectionString);

            return Task.FromResult<ITriggerBinding>(new RabbitMqTriggerAttributeBinding(channel, attribute.QueueName));
        }
    }
}
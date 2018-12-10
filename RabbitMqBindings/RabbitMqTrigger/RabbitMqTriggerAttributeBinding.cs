using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using RabbitMQ.Client;

namespace RabbitMqBindings.RabbitMqTrigger
{
    internal class RabbitMqTriggerAttributeBinding : ITriggerBinding
    {
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly IReadOnlyDictionary<string, object> _emptyBindingData = new Dictionary<string, object>();

        public RabbitMqTriggerAttributeBinding(IModel channel, string queueName)
        {
            _channel = channel;
            _queueName = queueName;
        }

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            return Task.FromResult<ITriggerData>(new TriggerData(null, _emptyBindingData));
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
        {
            return Task.FromResult<IListener>(new RabbitMqTriggerAttributeListener(_channel, _queueName, context.Executor));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor
            {
                Name = "message",
                Type = "string"
            };
        }

        public Type TriggerValueType { get; } = typeof(string);
        public IReadOnlyDictionary<string, Type> BindingDataContract { get; } = new Dictionary<string, Type>();
    }
}
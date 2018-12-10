using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RabbitMqBindings
{
    internal class ConsumerRabbitMqChannelFactory : RabbitMqChannelFactory
    {
        public ConsumerRabbitMqChannelFactory(ILogger<ConsumerRabbitMqChannelFactory> logger) : base(logger)
        {
        }

        protected override void ConfigureConnectionFactory(ConnectionFactory factory)
        {
            factory.DispatchConsumersAsync = true;
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace RabbitMqBindings
{

    internal class PublisherRabbitMqChannelFactory : RabbitMqChannelFactory
    {
        public PublisherRabbitMqChannelFactory(ILogger<PublisherRabbitMqChannelFactory> logger) : base(logger)
        {
        }
    }

}
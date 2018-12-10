using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace AzurePublishApi.Infrastructure
{
    public class RabbitMqChannelFactory: IDisposable
    {
        private readonly ILogger<RabbitMqChannelFactory> _logger;
        private readonly IConnection _connection;
        private readonly object syncRoot = new object();

        public RabbitMqChannelFactory(IOptions<RabbitMqOptions> options, ILogger<RabbitMqChannelFactory> logger)
        {
            _logger = logger;
            var connectionFactory = new ConnectionFactory
            {
                UserName = options.Value.Username,
                Password = options.Value.Password,
                HostName = options.Value.Hostname
            };
            _connection = connectionFactory.CreateConnection();
        }

        public IModel CreateChannel()
        {
            lock (syncRoot)
            {
                return _connection.CreateModel();
            }
        }

        public void Dispose()
        {
            _logger.LogDebug("Dispose channel factory");
            _connection.Dispose();
        }
    }
}
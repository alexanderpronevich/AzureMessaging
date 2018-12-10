using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RabbitMqBindings
{
    internal class RabbitMqChannelFactory
    {
        private readonly ILogger _logger;
        private readonly object syncRoot = new object();
        private readonly IDictionary<string, IConnection> factoryDictionary = new Dictionary<string, IConnection>();


        public RabbitMqChannelFactory(ILogger logger)
        {
            _logger = logger;
        }

        public IModel CreateChannel(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri));
            }

            lock (syncRoot)
            {
                if (factoryDictionary.TryGetValue(uri, out var connection))
                {
                    return connection.CreateModel();
                }

                var factory = new ConnectionFactory
                {
                };

                ConfigureConnectionFactory(factory);

                factory.Uri = new Uri(uri);

                connection = factory.CreateConnection();
                factoryDictionary[uri] = connection;

                return connection.CreateModel();
            }
        }

        protected virtual void ConfigureConnectionFactory(ConnectionFactory factory)
        {
        }

        public void Dispose()
        {
            _logger.LogDebug("Dispose channel factory");
            lock (syncRoot)
            {
                foreach (var conn in factoryDictionary.Values)
                {
                    conn.Dispose();
                }
                factoryDictionary.Clear();
            }
        }
    }
}
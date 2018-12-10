using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using RabbitMQ.Client;

namespace RabbitMqBindings.RabbitMqMessage
{
    internal class RabbitMqCollector : IAsyncCollector<string>
    {
        private readonly IModel _channel;
        private readonly string _exchangeName;
        private readonly string _routingKey;
        private readonly bool _mandatoryFlag;
        private readonly IBasicProperties _basicProperties;

        public RabbitMqCollector(IModel channel, string exchangeName, string routingKey, bool mandatoryFlag,
            byte deliveryMode)
        {
            _channel = channel;
            _exchangeName = exchangeName;
            _routingKey = routingKey;
            _mandatoryFlag = mandatoryFlag;
            _basicProperties = _channel.CreateBasicProperties();
            _basicProperties.DeliveryMode = deliveryMode;
        }

        public Task AddAsync(string item, CancellationToken cancellationToken = new CancellationToken())
        {
            _channel.BasicPublish(_exchangeName, _routingKey, _mandatoryFlag, _basicProperties, Encoding.UTF8.GetBytes(item));

            return Task.CompletedTask;
        }

        public Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _channel.Dispose();
            return Task.CompletedTask;
        }
    }
}
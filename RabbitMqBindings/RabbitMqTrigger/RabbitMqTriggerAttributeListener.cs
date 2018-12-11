using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqBindings.RabbitMqTrigger
{
    internal class RabbitMqTriggerAttributeListener : IListener
    {
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly ITriggeredFunctionExecutor _contextExecutor;
        private string _consumeTag;

        public RabbitMqTriggerAttributeListener(IModel channel, string queueName, ITriggeredFunctionExecutor contextExecutor)
        {
            _channel = channel;
            _queueName = queueName;
            _contextExecutor = contextExecutor;
        }

        public void Dispose()
        {
            _channel.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, e) =>
            {
                var data = Encoding.UTF8.GetString(e.Body);
                var result = await _contextExecutor.TryExecuteAsync(new TriggeredFunctionData {TriggerValue = data},
                    cancellationToken);
                if (result.Succeeded)
                {
                    _channel.BasicAck(e.DeliveryTag, false);
                }
                else
                {
                    _channel.BasicNack(e.DeliveryTag, false, false);
                }
            };
            _consumeTag = _channel.BasicConsume(_queueName, false, consumer);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.BasicCancel(_consumeTag);
            return Task.CompletedTask;
        }

        public void Cancel()
        {
            _channel.Dispose();
        }
    }
}
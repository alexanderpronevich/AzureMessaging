using System;
using System.Text;
using AzurePublishApi.Infrastructure;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace AzurePublishApi.Model
{
    public class RabbitMqMessageStore: IMessageStore, IDisposable
    {
        private readonly ILogger<RabbitMqMessageStore> _logger;
        private readonly IModel _channel;
        private readonly IBasicProperties _props;

        public RabbitMqMessageStore(RabbitMqChannelFactory channelFactory, ILogger<RabbitMqMessageStore> logger)
        {
            _logger = logger;
            _channel = channelFactory.CreateChannel();
            _props = _channel.CreateBasicProperties();
            _props.DeliveryMode = 2;
            _props.ContentType = "plain/text";
        }

        public void PublishStageOneMessage(string message)
        {
            _logger.LogInformation("stage 1 message");
            _channel.BasicPublish("stagedPublishExchange", "stage.one.message", true, _props,
                Encoding.UTF8.GetBytes(message));
        }

        public void PublishStageTwoMessage(string message)
        {
            _channel.BasicPublish("stagedPublishExchange", "stage.two.message", true, _props,
                Encoding.UTF8.GetBytes(message));
        }

        public void PublishStageThreeMessage(string message)
        {
            _channel.BasicPublish("stagedPublishExchange", "stage.three.message", true, _props,
                Encoding.UTF8.GetBytes(message));
        }

        public void Dispose()
        {
            _logger.LogDebug("Dispose store");
            _channel.Dispose();
        }
    }
}
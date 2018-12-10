using System;
using Microsoft.Azure.WebJobs.Description;

namespace RabbitMqBindings.RabbitMqMessage
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    [Binding]
    public class RabbitMqMessageAttribute: Attribute
    {
        public string ExchangeName { get; }
        public string RoutingKey { get; }
        public bool Mandatory { get; }

        public string ConnectionStringSetting { get; set; }

        public byte DeliveryMode { get; set; } = 2;

        public RabbitMqMessageAttribute(string exchangeName, string routingKey, bool mandatory=false)
        {
            ExchangeName = exchangeName;
            RoutingKey = routingKey;
            Mandatory = mandatory;
        }
    }
}
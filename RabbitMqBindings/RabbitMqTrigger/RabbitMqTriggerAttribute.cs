using System;
using Microsoft.Azure.WebJobs.Description;

namespace RabbitMqBindings.RabbitMqTrigger
{
    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class RabbitMqTriggerAttribute:Attribute
    {
        public string QueueName { get; }

        public string ConnectionStringSetting { get; set; }

        public RabbitMqTriggerAttribute(string queueName)
        {
            QueueName = queueName;
        }
    }
}
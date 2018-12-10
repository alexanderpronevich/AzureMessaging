using Microsoft.Azure.WebJobs;
using RabbitMqBindings.RabbitMqMessage;
using RabbitMqBindings.RabbitMqTrigger;

namespace QueueWorker
{
    public class Functions
    {
        public void OnStageOneMessage(
            [RabbitMqTrigger("stage_one", ConnectionStringSetting = "RabbitMq")]
            string message,
            [RabbitMqMessage("stagedPublishExchange", "stage.two.message", ConnectionStringSetting = "RabbitMq")]
            out string nextMessage)
        {
            nextMessage = message;
        }

        public void OnStageTwoMessage(
            [RabbitMqTrigger("stage_two", ConnectionStringSetting = "RabbitMq")]
            string message, 
            [RabbitMqMessage("stagedPublishExchange", "stage.three.message", ConnectionStringSetting = "RabbitMq")]
            out string nextMessage)
        {
            nextMessage = message;
        }

        public void OnStageThreeMessage(
            [RabbitMqTrigger("stage_three", ConnectionStringSetting = "RabbitMq")]
            string message,
            [CosmosDB("MessageDb", "MessageCollection", ConnectionStringSetting = "ConnectionStrings:CosmosDb")]
            out CosmosDbStoreObject messageToStore)
        {
            messageToStore = new CosmosDbStoreObject {Message = message};
        }
    }

    public class CosmosDbStoreObject
    {
        public string Message { get; set; }
    }
}
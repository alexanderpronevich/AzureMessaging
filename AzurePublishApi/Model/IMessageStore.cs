namespace AzurePublishApi.Model
{
    public interface IMessageStore
    {
        void PublishStageOneMessage(string message);
        void PublishStageTwoMessage(string message);
        void PublishStageThreeMessage(string message);
    }
}
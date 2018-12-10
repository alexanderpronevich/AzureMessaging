using AzurePublishApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace AzurePublishApi.Controllers
{
    [Route("/publish")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly IMessageStore _messageStore;

        public PublishController(IMessageStore messageStore)
        {
            _messageStore = messageStore;
        }


        [Route("stage_one")]
        [HttpPost]
        public void StageOne([FromBody] string message)
        {
            _messageStore.PublishStageOneMessage(message);
        }

        [Route("stage_two")]
        [HttpPost]
        public void StageTwo([FromBody] string message)
        {
            _messageStore.PublishStageTwoMessage(message);
        }

        [Route("stage_three")]
        [HttpPost]
        public void StageThree([FromBody] string message)
        {
            _messageStore.PublishStageThreeMessage(message);
        }
    }
}

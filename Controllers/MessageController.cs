using Koshelekpy_Test.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Koshelekpy_Test.Application;
using Microsoft.AspNetCore.SignalR;
using Koshelekpy_Test.Infrastracture.UOW;

namespace Koshelekpy_Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        //DI components
        private readonly IHubContext<MessageHub> _hubContext;
        private readonly ILogger _logger;
        private readonly UnitOfWork _unitOfWork;

        //Message event
        private delegate void MessageHandler(Message message);
        private event MessageHandler OnMessageSend;


        public MessagesController(IUnitOfWork unitOfWork , IHubContext<MessageHub> hubContext, ILogger<MessagesController> logger)
        {
            OnMessageSend += MessageHahdle;
            _unitOfWork = (UnitOfWork)unitOfWork;
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpPost]
        [Route("/SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            try
            {
                if (message == null || string.IsNullOrEmpty(message.Text))
                {
                    return BadRequest("Invalid message.");
                }

                _unitOfWork.CreateTransaction();
                var taskguid = await _unitOfWork.Messages.Create(message);
                await _unitOfWork.Save();
                _unitOfWork.EndTransaction();

                OnMessageSend.Invoke(message);

                _logger.LogInformation($"{nameof(_unitOfWork.Messages.Create)} executed correctly with Id: {taskguid}");
                return Ok();
            }
            catch (Exception ex) 
            {
                _logger.LogInformation($"{nameof(_unitOfWork.Messages.Create)} executed incorrectly ({ex.Message})");

                _unitOfWork.Rollback();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/GetMessages")]
        public async Task<ActionResult<List<Message>>> GetMessages([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            try
            {
                _unitOfWork.CreateTransaction();

                var messages = await _unitOfWork.Messages.GetAsync(from, to);

                return messages;
            }
            catch (Exception ex) 
            {
                _logger.LogInformation($"{nameof(_unitOfWork.Messages.Create)} executed incorrectly ({ex.Message})");

                _unitOfWork.Rollback();
                return BadRequest([]);
            }
        }

        private async void MessageHahdle(Message message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message.Text, message.SequenceNumber, message.Timestamp.ToUniversalTime());
        }
    }
}

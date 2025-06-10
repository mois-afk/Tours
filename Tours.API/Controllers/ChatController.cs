using Microsoft.AspNetCore.Mvc;
using Tours.Service.Interface;
using Tours.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Tours.API.Models;

namespace Tours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private readonly IRedisService _redisService;

        public ChatController(IChatService chatService, IRedisService redisService)
        {
            _chatService = chatService;
            _redisService = redisService;
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _chatService.GetAllMessage();
            return Ok(messages);
        }

        [HttpPost("send-messages")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Text))
            {
                return BadRequest();
            }

            await _chatService.AddMessage(model.Text, model.Username);
            return Ok(new { Message = "Сообщение успешно отправлено" });
        }

        [HttpPost("messages/{messageId}/like")]
        [Authorize]
        public async Task<IActionResult> LikeMessage([FromBody] LikeModel model)
        {
            if (string.IsNullOrEmpty(model.MessageId))
            {
                return BadRequest();
            }

            await _chatService.LikeMessage(model.MessageId, model.Username);
            return Ok();
        }

        [HttpPost("messages/{messageId}/dislike")]
        [Authorize]
        public async Task<IActionResult> DislikeMessage([FromBody] LikeModel model)
        {
            if (string.IsNullOrEmpty(model.MessageId))
            {
                return BadRequest();
            }

            await _chatService.DislikeMessage(model.MessageId, model.Username);
            return Ok(new { Message = "Дизлайк добавлен" });
        }

        [HttpPost("messages/{messageId}/unlike")]
        [Authorize][Authorize]
        public async Task<IActionResult> UnLikeMessage([FromBody] LikeModel model)
        {
            if (string.IsNullOrEmpty(model.MessageId))
            {
                return BadRequest();
            }

            await _chatService.UnLikeMessage(model.MessageId, model.Username);
            return Ok();
        }

        [HttpPost("messages/{messageId}/undislike")]
        public async Task<IActionResult> UnDislikeMessage([FromBody] LikeModel model)
        {
            if (string.IsNullOrEmpty(model.MessageId))
            {
                return BadRequest();
            }

            await _chatService.UnDislikeMessage(model.MessageId, model.Username);
            return Ok();
        }

        [HttpPost("messages/{messageId}/comment")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] CommentModel model)
        {
            if (string.IsNullOrEmpty(model.MessageId) || model == null || string.IsNullOrEmpty(model.Text))
            {
                return BadRequest();
            }

            await _chatService.AddComment(model.MessageId, model.Text, model.Username);
            return Ok();
        }
    }
}
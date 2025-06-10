using Microsoft.AspNetCore.Mvc;
using Tours.Service.Interface;
using Tours.Models;
using System.Text;
using System.Threading.Tasks;
using Tours.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Tours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IRedisService _redisService;
        private readonly IEmailService _emailService;

        public EmailController(IRedisService redisService, IEmailService emailService)
        {
            _redisService = redisService;
            _emailService = emailService;
        }

        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Subject) || string.IsNullOrEmpty(model.Body))
            {
                return BadRequest();
            }

            var email = HttpContext.Items["email"]?.ToString();
            if (_emailService.SendEmail(email, model.Subject, model.Body))
            {
                if (await _emailService.SaveEmail(email, model.Body, DateTime.Now))
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpPost("order-details")]
        [Authorize]
        public IActionResult SendEmailOrderDetail([FromBody] OrderModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || model.TotalPrice < 0)
            {
                return BadRequest();
            }

            var email = HttpContext.Items["email"]?.ToString();

            StringBuilder order = new StringBuilder();
            order.AppendLine("Детали заказа");
            order.AppendLine($"Пользователь: {model.Username}");
            order.AppendLine($"Дата заказа: {model.Date}");
            order.AppendLine($"Общая стоимость заказа: {model.TotalPrice}");

            string body = order.ToString();

            if (_emailService.SendEmail(email, "Детали заказа", body))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
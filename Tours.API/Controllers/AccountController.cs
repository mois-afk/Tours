namespace Tours.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Tours.Service.Interface;
    using Tours.Models;
    using Tours.Service;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tours.API.Models;
    using Microsoft.AspNetCore.Authorization;

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRedisService _redisService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IMailingService _mailingService;
        private readonly PdfService _pdfService;

        public AccountController(
            IRedisService redisService,
            IUserService userService,
            IOrderService orderService,
            IMailingService mailingService,
            PdfService pdfService)
        {
            _redisService = redisService;
            _userService = userService;
            _orderService = orderService;
            _mailingService = mailingService;
            _pdfService = pdfService;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var email = HttpContext.Items["email"]?.ToString();


            var user = await _userService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }


            return Ok(user);
        }

        [HttpGet("orders")]
        [Authorize]
        public async Task<IActionResult> GetOrders()
        {
            var email = HttpContext.Items["email"]?.ToString();
            var orders = await _orderService.GetAllOrdersByEmail(email);
            return Ok(orders);
        }

        [HttpGet("orders-pdf")]
        [Authorize]
        public async Task<IActionResult> GetOrderPDF()
        {
            var email = HttpContext.Items["email"]?.ToString();

            var orders = await _orderService.GetAllOrdersByEmail(email);
            var pdfBytes = _pdfService.CreateUserOrdersReport(orders);
            return File(pdfBytes, "application/pdf", "OrdersReport.pdf");
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Username))
            {
                return BadRequest();
            }

            var email = HttpContext.Items["email"]?.ToString();

            var user = await _userService.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }

            model.UserId = user.UserId;
            await _userService.UpdateUser(model);
            return Ok();
        }

        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword))
            {
                return BadRequest();
            }

            var email = HttpContext.Items["email"]?.ToString();

            var errors = new Dictionary<string, string>();
            if (await _userService.UpdatePassword(model.NewPassword, model.CurrentPassword))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            //_redisService.Clear();
            return Ok();
        }
    }
}
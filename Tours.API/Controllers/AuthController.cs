namespace Tours.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Tours.Service.Interface;
    using Tours.Service;
    using Tours.Models;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Tours;
    using Tours.API.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IRedisService _redisService;

        public AuthController(IAuthenticationService authenticationService, IUserService userService, IRedisService redisService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _redisService = redisService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest();
            }

            var token = await _authenticationService.AuthenticateAsync(model.Email, model.Password);

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Email))
            {
                return BadRequest();
            }

            var token = await _authenticationService.RegistrationAsync(model.Username, model.Password, model.Email);

            return Ok(token);
        }

        [HttpGet("verify-email")]
        public IActionResult VerifyEmail([FromQuery] string code)
        {
            var storedCode = _redisService.Get<string>("Code");
            if (storedCode == code)
            {
                return Ok(new { Status = "success" });
            }
            return BadRequest(new { Status = "failure", Message = "Неверный код подтверждения" });
        }

        [HttpPost("send-verification-code")]
        public async Task<IActionResult> SendCodeToEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Message = "Email обязателен" });
            }

            var code = await _authenticationService.SendVerifyCodeToEmailAsync(email);
            if (code != null)
            {
                _redisService.Set("Code", code);
                return Ok(new { Success = true, Email = email });
            }

            return BadRequest();
        }
    }
}
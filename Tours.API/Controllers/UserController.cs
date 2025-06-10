using Microsoft.AspNetCore.Mvc;
using Tours;
using Tours.Service.Interface;
using Tours.Models;
using Microsoft.AspNetCore.Authorization;

namespace Tours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRedisService _redisService;
        private readonly IAdminService _adminService;

        public UserController(IUserService userService, IRedisService redisService, IAdminService adminService)
        {
            _userService = userService;
            _redisService = redisService;
            _adminService = adminService;
        }

        [HttpGet("search")]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> SearchUser([FromQuery] string query)
        {
            List<User> users;

            if (string.IsNullOrEmpty(query))
            {
                users = await _userService.GetAllUsers();
            }
            else
            {
                users = await _userService.GetAllUsersByUsername(query);
            }

            return Ok(users);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Email) || model.UserId == null)
            {
                return BadRequest();
            }

            await _userService.UpdateUser(model);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            await _adminService.DeleteUser(userId);

            return Ok();
        }
    }
}
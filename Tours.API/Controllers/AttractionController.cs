using Microsoft.AspNetCore.Mvc;
using Tours;
using Tours.Service.Interface;
using Tours.Models;
using Tours.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttractionController : ControllerBase
    {
        private readonly IAttractionService _attractionService;
        private readonly IRedisService _redisService;
        private readonly ICityService _cityService;
        private readonly IAdminService _adminService;

        public AttractionController(
            IAttractionService attractionService,
            IRedisService redisService,
            ICityService cityService,
            IAdminService adminService)
        {
            _attractionService = attractionService;
            _redisService = redisService;
            _cityService = cityService;
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var attractions = await _attractionService.GetAllAttractionAsync();
            return Ok(attractions);
        }

        [HttpGet("by-city")]
        public async Task<IActionResult> GetAttractionsByCity([FromQuery] string cityId)
        {
            if (string.IsNullOrEmpty(cityId))
            {
                return BadRequest();
            }

            var attractions = await _attractionService.GetAllAttractionsByCityIdAsync(cityId);
            return Ok(attractions);
        }

        [HttpPost]
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> AddAttraction([FromBody] AttractionModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.AttractionName) || string.IsNullOrEmpty(model.CityId))
            {
                return BadRequest();
            }

            if (await _attractionService.AddAttractionAsync(model))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAttraction([FromQuery] string query)
        {
            List<Attraction> attractions;

            if (string.IsNullOrEmpty(query))
            {
                attractions = await _attractionService.GetAllAttractionAsync();
            }
            else
            {
                attractions = await _attractionService.GetAllAtractionByPartNameAsync(query);
            }

            return Ok(attractions);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAttraction([FromBody] AttractionModel model)
        {
            if (model == null || model.AttractionId == null || string.IsNullOrEmpty(model.AttractionName) || string.IsNullOrEmpty(model.CityId))
            {
                return BadRequest();
            }

            if (await _attractionService.UpdateAttractionAsync(model))
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAttraction([FromQuery] string attId)
        {
            if (attId == null)
            {
                return BadRequest();
            }

            await _adminService.DeleteAttractionAsync(attId);

            return Ok();
        }
    }
}
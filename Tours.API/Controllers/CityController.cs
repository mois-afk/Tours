using Microsoft.AspNetCore.Mvc;
using Tours;
using Tours.Service.Interface;
using Tours.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;
        private readonly IRedisService _redisService;
        private readonly IAdminService _adminService;

        public CityController(ICityService cityService, IRedisService redisService, IAdminService adminService)
        {
            _cityService = cityService;
            _redisService = redisService;
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _cityService.GetAllCity();
            return Ok(cities);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCity([FromQuery] string query)
        {
            List<City> cities;

            if (string.IsNullOrEmpty(query))
            {
                cities = await _cityService.GetAllCity();
            }
            else
            {
                cities = await _cityService.GetAllCityByPartName(query);
            }

            return Ok(cities);
        }

        [HttpPost("add-cities")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCity([FromBody] CityModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.CityName) || string.IsNullOrEmpty(model.CityDescription) || string.IsNullOrEmpty(model.URL))
            {
                return BadRequest();
            }

            await _cityService.AddCity(model);

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCity([FromBody] CityModel model)
        {
            if (model == null || model.CityId == null || string.IsNullOrEmpty(model.CityName) || string.IsNullOrEmpty(model.CityDescription) || string.IsNullOrEmpty(model.URL))
            {
                return BadRequest();
            }

            await _cityService.UpdateCityAsync(model);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCity([FromQuery] string cityId)
        {
            if (cityId == null)
            {
                return BadRequest();
            }

            await _adminService.DeleteCity(cityId);

            return Ok();
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Tours.Service.Interface;
using Tours.Models;
using System.Threading.Tasks;

namespace Tours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        private readonly IRedisService _redisService;

        public SearchController(ISearchService searchService, IRedisService redisService)
        {
            _searchService = searchService;
            _redisService = redisService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] SearchModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var result = await _searchService.SearchResultAsync(model);
            return Ok(result);
        }
    }
}
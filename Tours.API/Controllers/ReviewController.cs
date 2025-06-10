using Microsoft.AspNetCore.Mvc;
using Tours;
using Tours.Service.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Tours.API.Models;

namespace Tours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IRedisService _redisService;
        private readonly ITourService _tourService;

        public ReviewController(IReviewService reviewService, IRedisService redisService, ITourService tourService)
        {
            _reviewService = reviewService;
            _redisService = redisService;
            _tourService = tourService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        [HttpGet("tours")]
        public async Task<IActionResult> GetToursForReview()
        {
            var tours = await _tourService.GetAllToursAsync();
            return Ok(tours);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview([FromBody] AddReviewModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.ReviewText) || string.IsNullOrEmpty(model.TourId))
            {
                return BadRequest();
            }

            await _reviewService.AddReviewAsync(model.ReviewText, model.TourId, model.Username);
            return Ok();
        }
    }
}
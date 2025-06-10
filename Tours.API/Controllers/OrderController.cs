using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tours.Service.Interface;
using Tours.Models;

namespace Tours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IRedisService _redisService;
        private readonly IOrderService _orderService;
        private readonly ITourService _tourService;

        public OrderController(IOrderService orderService, IRedisService redisService, ITourService tourService)
        {
            _redisService = redisService;
            _orderService = orderService;
            _tourService = tourService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
            var tourIdList = _redisService.Get<List<string>>("TourIdList") ?? new List<string>();
            var tours = await _tourService.GetAllToursByAllIdAsync(tourIdList);
            var totalPrice = _orderService.CalculateTotalPrice(tours);

            return Ok(new { Tours = tours, TotalPrice = totalPrice });
        }

        [HttpPost("add-tour")]
        public IActionResult AddToOrder([FromQuery] string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
            {
                return BadRequest();
            }

            _orderService.AddTourToOrder(tourId);
            return Ok();
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> AddOrder([FromQuery] string username)
        {
            var order = await _orderService.CreateOrder(username);
            _redisService.Set("order", order);
            return Ok(order);
        }

        [HttpPost("buy-tour")]
        [Authorize]
        public async Task<IActionResult> BuyMyTour([FromBody] TourModel model, [FromQuery] string username)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var order = await _orderService.BuyMyTour(model, username);
            _redisService.Set("order", order);
            return Ok();
        }

        [HttpPost("confirm")]
        [Authorize]
        public async Task<IActionResult> AddOrderResult([FromBody] OrderModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }


            await _orderService.AddOrderAsync(model);
            return Ok();
        }

        [HttpDelete("remove-tour")]
        public IActionResult RemoveTour([FromQuery] string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
            {
                return BadRequest();
            }


            _orderService.RemoveTourFromOrder(tourId);
            return Ok();
        }
    }
}
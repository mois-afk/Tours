using Microsoft.AspNetCore.Mvc;
using Tours;
using Tours.Service.Interface;
using Tours.Models;
using Tours.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourController : ControllerBase
    {
        private readonly ITourService _tourService;
        private readonly IAttractionService _attractionService;
        private readonly IRedisService _redisService;
        private readonly PdfService _pdfService;
        private readonly IOrderService _orderService;

        public TourController(ITourService tourService, IRedisService redisService, IAttractionService attractionService, PdfService pdfService, IOrderService orderService)
        {
            _tourService = tourService;
            _redisService = redisService;
            _attractionService = attractionService;
            _pdfService = pdfService;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTours()
        {
            var tours = await _tourService.GetAllToursAsync();
            return Ok(tours);
        }

        [HttpGet("price-list")]
        public async Task<IActionResult> GetPriceListPDF()
        {
            var tours = await _tourService.GetAllToursAsync();
            var pdfBytes = _pdfService.CreateTourPriceList(tours);
            return File(pdfBytes, "application/pdf", "PriceList.pdf");
        }

        [HttpGet("{tourId}/details")]
        public async Task<IActionResult> GetTourDetails([FromQuery] string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
            {
                return BadRequest();
            }

            var attractions = await _tourService.GetAttractionDateForTour(tourId);
            var result = attractions.Select(a => new AttractionVisitDto
            {
                Attraction = a.Key,
                VisitDate = a.Value
            }).ToList();

            return Ok(result);
        }

        public class AttractionVisitDto
        {
            public Attraction Attraction { get; set; }
            public DateTime VisitDate { get; set; }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTour([FromQuery] string query)
        {
            List<Tour> tours;

            if (string.IsNullOrEmpty(query))
            {
                tours = await _tourService.GetAllToursAsync();
            }
            else
            {
                tours = await _tourService.GetAllTourByPartNameAsync(query);
            }

            return Ok(tours);
        }

        [HttpPut("update-tour")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTour([FromBody] TourModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.TourId) || string.IsNullOrEmpty(model.TourName) || string.IsNullOrEmpty(model.TourDescription))
            {
                return BadRequest();
            }

            await _tourService.UpdateTourAsync(model);

            return Ok();
        }

        [HttpPost("add-tour")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTour([FromBody] TourModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.TourName) || string.IsNullOrEmpty(model.TourDescription))
            {
                return BadRequest();
            }

            await _tourService.AddTourAsync(model);

            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTour([FromBody] TourModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var newTourId = await _tourService.AddTourAsync(model, true);
            _orderService.AddTourToOrder(newTourId);
            return Ok(newTourId);
        }

        [HttpPost("my-tour")]
        [Authorize]
        public async Task<IActionResult> AddToMyTour([FromQuery] string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
            {
                return BadRequest();
            }

            var myTourList = await _tourService.AddToMyTour(tourId);
            _redisService.Set("MyTourList", myTourList);
            return Ok(new { Success = true });
        }
    }
}
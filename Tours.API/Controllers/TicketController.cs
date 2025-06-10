using Microsoft.AspNetCore.Mvc;
using Tours.Service.Interface;
using System.Threading.Tasks;

namespace Tours.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("tikets")]
        public async Task<IActionResult> GetTickets([FromQuery] string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
            {
                return BadRequest(new { Message = "Идентификатор тура обязателен" });
            }

            var tickets = await _ticketService.GetTicketsForTourAsync(tourId);
            return Ok(tickets);
        }
    }
}
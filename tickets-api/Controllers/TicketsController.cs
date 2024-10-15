using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tickets_api.Models;
using tickets_api.Repositories;

namespace tickets_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController(ITicketRepository ticketRepository) : Controller
    {
        private readonly ITicketRepository _ticketRepository = ticketRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }

            var results = await _ticketRepository.GetAllTicketsAsync(pageNumber, pageSize);

            var paginationMetadata = new
            {
                totalCount = results.TotalCount,
                pageSize,
                currentPage = pageNumber,
                totalPages = (int)Math.Ceiling(results.TotalCount / (double)pageSize)
            };

            return Ok(new
            {
                results.data,
                pagination = paginationMetadata
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicket(Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ticket.CreatedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.Status = TicketStatus.Open;
            await _ticketRepository.AddTicketAsync(ticket);

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            try
            {
                await _ticketRepository.UpdateTicketAsync(ticket);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_ticketRepository.TicketExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok("Updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            if (_ticketRepository.TicketExists(id))
            {
                await _ticketRepository.DeleteTicketAsync(id);
                return Ok("Deleted successfully");
            }

            return NotFound();
        }
    }
}

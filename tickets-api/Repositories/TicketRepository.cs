using Microsoft.EntityFrameworkCore;
using tickets_api.Data;
using tickets_api.Models;

namespace tickets_api.Repositories
{
    public class TicketRepository(ApplicationDbContext context) : ITicketRepository
    {
        private readonly ApplicationDbContext _context = context;

        // Get all tickets with pagination
        public async Task<PagedResult<Ticket>> GetAllTicketsAsync(int pageNumber, int pageSize)
        {
            var totalTickets = await _context.Tickets.CountAsync();
            var tickets = await _context.Tickets
                .OrderByDescending(t => t.UpdatedAt) // Order by UpdatedAt
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Ticket>
            {
                data = tickets,
                TotalCount = totalTickets
            };
        }

        // Get a ticket by ID
        public async Task<Ticket> GetTicketByIdAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id) ?? throw new KeyNotFoundException($"Ticket with id {id} not found.");
            return ticket;
        }

        // Add a new ticket
        public async Task AddTicketAsync(Ticket ticket)
        {
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.UpdatedAt = DateTime.UtcNow;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        // Update an existing ticket
        public async Task UpdateTicketAsync(Ticket ticket)
        {
            var existingTicket = await _context.Tickets.FindAsync(ticket.Id);
            if (existingTicket != null)
            {
                existingTicket.Description = ticket.Description;
                existingTicket.Status = ticket.Status;
                existingTicket.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        // Delete a ticket by ID
        public async Task DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        public bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}

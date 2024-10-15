using tickets_api.Models;

namespace tickets_api.Repositories
{
    public interface ITicketRepository
    {
        Task<PagedResult<Ticket>> GetAllTicketsAsync(int pageNumber, int pageSize);
        Task<Ticket> GetTicketByIdAsync(int id);
        Task AddTicketAsync(Ticket ticket);
        Task UpdateTicketAsync(Ticket ticket);
        Task DeleteTicketAsync(int id);
        bool TicketExists(int id);
    }
}

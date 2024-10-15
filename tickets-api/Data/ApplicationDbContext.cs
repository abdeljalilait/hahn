using Microsoft.EntityFrameworkCore;
using tickets_api.Models;

namespace tickets_api.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Register PostgreSQL enum type for TicketStatus
            builder.HasPostgresEnum<TicketStatus>();
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace tickets_api.Models
{
    public enum TicketStatus
    {
        Open,
        Closed
    }

    public class PagedResult<T>
    {
        public required List<T> data { get; set; }
        public int TotalCount { get; set; }
    }


    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TicketStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccalendar.Models.ViewModels.Events
{
    public class EventDetails
    {
        public int EventId { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public int DurationMinutes => End.HasValue ? (int)(End.Value - Start).TotalMinutes : 0;
        public string Title { get; set; } = string.Empty;
        public string? Color { get; set; }
        public string? Notes { get; set; }

        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public DateTime? LastContact { get; set; }
    }
}
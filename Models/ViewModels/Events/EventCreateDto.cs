using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccalendar.Models.ViewModels.Events
{
    public class EventCreateDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerSurname { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Duration { get; set; }
        public bool AllDay { get; set; }
        public string? Color { get; set; }
        public string? Notes { get; set; }
    }
}
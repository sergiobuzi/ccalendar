using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccalendar.Models.ViewModels.Events
{
    public class EventListViewModel
    {
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public int DurationMinutes => End.HasValue ? (int)(End.Value - Start).TotalMinutes : 0;
        public bool AllDay { get; set; }
        public string? Color { get; set; }
        public string? Notes { get; set; }
    }
}
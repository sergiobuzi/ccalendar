using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ccalendar.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        [MaxLength(64)]
        public string Title { get; set; }

        public DateTime Start { get; set; }
        public DateTime? End { get; set; }

        public bool AllDay { get; set; }

        [MaxLength(16)]
        public string? Color { get; set; }

        public string? Notes { get; set; }

        public bool IsVisit { get; set; } = false;

        [MaxLength(32)]
        public string? EventType { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ccalendar.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [MaxLength(24)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(24)]
        public string Surname { get; set; } = string.Empty;

        [MaxLength(16)]
        public string Mobile { get; set; } = string.Empty;

        [MaxLength(64)]
        public string Email { get; set; } = string.Empty;

        public bool ToCall { get; set; }
        public bool Active { get; set; }

        public DateTime? LastContact { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastVisitDate { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
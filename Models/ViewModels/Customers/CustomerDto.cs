using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ccalendar.Models.ViewModels.Events;

namespace ccalendar.Models.ViewModels.Customers
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime? LastContact { get; set; }
        public string? Notes { get; set; }
        public ICollection<EventListViewModel> Events { get; set; }
    }
}
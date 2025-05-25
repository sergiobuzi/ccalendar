using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ccalendar.Models.ViewModels.Customers
{
    public class CustomerListViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool ToCall { get; set; }
        public DateTime? LastContact { get; set; }
    }
}
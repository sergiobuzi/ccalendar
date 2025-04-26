using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ccalendar.Models.ViewModels.Customers;
using ccalendar.Models.ViewModels.Events;

namespace ccalendar.Models.ViewModels.Home
{
    public class DashboardViewModel
    {
        public List<CustomerListViewModel> Customers { get; set; }
        public List<EventListViewModel> Events { get; set; }
        
    }
}
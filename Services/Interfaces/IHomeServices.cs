using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ccalendar.Data;
using ccalendar.Models;
using ccalendar.Models.ViewModels.Customers;
using ccalendar.Models.ViewModels.Events;
using Microsoft.AspNetCore.Mvc;

namespace ccalendar.Services.Interfaces
{
    public interface IHomeServices
    {
        Task<List<CustomerListViewModel>> GetCustomersToRecall();
        Task<bool> CreateEvent(EventCreateDto model);
        Task<JsonResult> GetEvents();
        Task<JsonResult> SearchCustomers(string text);
        Task<JsonResult> GetCustomerById(int id);
    }
}
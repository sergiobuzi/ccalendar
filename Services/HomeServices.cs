using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ccalendar.Data;
using ccalendar.Models;
using ccalendar.Models.ViewModels.Customers;
using ccalendar.Models.ViewModels.Events;
using ccalendar.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ccalendar.Services
{
    public class HomeServices : IHomeServices
    {
        private readonly ApplicationDbContext _context;

        public HomeServices( ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<CustomerListViewModel>> GetCustomersToRecall()
        {
            try
            {
                List<Customer> model = await _context.Customers
                    .Where(c => c.Active == true && c.ToCall == true)
                    .ToListAsync();

                List<CustomerListViewModel> customersToRecall = new List<CustomerListViewModel>();
                foreach(var customer in model)
                {
                    customersToRecall.Add(new CustomerListViewModel
                    {
                        CustomerId = customer.CustomerId,
                        Name = customer.Name,
                        Surname = customer.Surname,
                        Mobile = customer.Mobile,
                        Email = customer.Email,
                        ToCall = customer.ToCall,
                        LastVisitDate = customer.LastVisitDate
                    });
                }
                return customersToRecall;
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        public async Task<bool> CreateEvent(EventCreateDto dto)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == dto.CustomerId);

                if (customer == null)
                    return false;

                var localStart = TimeZoneInfo.ConvertTimeFromUtc(
                    dto.Start.ToUniversalTime(),
                    TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
                    
                var localEnd = TimeZoneInfo.ConvertTimeFromUtc(
                    dto.End.ToUniversalTime(),
                    TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));

                Event model = new Event()
                {
                    CustomerId = dto.CustomerId,
                    Title = dto.Title,
                    Start = localStart,
                    End = localEnd,
                    AllDay = dto.AllDay,
                    Color = dto.Color,
                    Notes = dto.Notes
                };

                await _context.Events.AddAsync(model);
                customer.ToCall = false;
                customer.LastContact = DateTime.Now;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        public async Task<JsonResult> GetEvents()
        {
            try
            {
                var events = await _context.Events
                    .Select(e => new {
                        id = e.EventId,
                        title = e.Title,
                        start = e.Start,
                        end = e.End,
                        allDay = e.AllDay
                    }).ToListAsync();

                return new JsonResult(events);
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

    }
}
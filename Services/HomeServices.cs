using System;
using System.Collections.Generic;
using System.Drawing;
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
                    .Where(c => c.Active == true && c.LastContact < DateTime.Now.AddMonths(-5).AddDays(-15))
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

        public async Task<int> CreateEvent(EventCreateDto dto)
        {
            try
            {
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == dto.CustomerId);

                if (customer == null)
                    return 0;

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

                return model.EventId;
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        public async Task<int> CreateCustomerAndEvent(EventCreateDto dto)
        {
            try
            {
                Customer customer = new Customer()
                {
                    Name = dto.CustomerName,
                    Surname = dto.CustomerSurname,
                    Email = dto.CustomerEmail,
                    Mobile = dto.CustomerPhone,
                    ToCall = false,
                    Active = true,
                    Notes = dto.Notes,
                    LastContact = DateTime.Now,
                    CreatedAt = DateTime.Now,
                };

                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();

                var localStart = TimeZoneInfo.ConvertTimeFromUtc(
                    dto.Start.ToUniversalTime(),
                    TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));
                    
                var localEnd = TimeZoneInfo.ConvertTimeFromUtc(
                    dto.End.ToUniversalTime(),
                    TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time"));

                Event model = new Event()
                {
                    CustomerId = customer.CustomerId,
                    Title = dto.Title,
                    Start = localStart,
                    End = localEnd,
                    AllDay = dto.AllDay,
                    Color = dto.Color,
                    Notes = dto.Notes
                };

                await _context.Events.AddAsync(model);
                await _context.SaveChangesAsync();

                return model.EventId;
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
                        allDay = e.AllDay,
                        color = e.Color
                    }).ToListAsync();

                return new JsonResult(events);
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        public async Task<List<CustomerListViewModel>> GetCustomers()
        {
            try
            {
                List<Customer> model = await _context.Customers
                    .Where(c => c.Active == true)
                    .ToListAsync();

                List<CustomerListViewModel> customers = new List<CustomerListViewModel>();
                foreach(var customer in model)
                {
                    customers.Add(new CustomerListViewModel
                    {
                        CustomerId = customer.CustomerId,
                        Name = customer.Name,
                        Surname = customer.Surname,
                        Mobile = customer.Mobile,
                        Email = customer.Email,
                        ToCall = customer.ToCall,
                        IsActive = customer.Active,
                        LastContact = customer.LastContact,
                        LastVisitDate = customer.LastVisitDate
                    });
                }
                return customers;
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        public async Task<JsonResult> EventDetails(int id)
        {
            try
            {
                var eventDetails = await _context.Events
                    .Where(e => e.EventId == id)
                    .Select(e => new
                    {
                        id = e.EventId,
                        title = e.Title,
                        start = e.Start,
                        end = e.End,
                        allDay = e.AllDay,
                        notes = e.Notes,
                        customerId = e.CustomerId
                    }).FirstOrDefaultAsync();

                var customerDetails = await _context.Customers
                    .Where(c => c.CustomerId == eventDetails.customerId)
                    .Select(c => new
                    {
                        name = c.Name,
                        surname = c.Surname,
                        phone = c.Mobile,
                        email = c.Email
                    }).FirstOrDefaultAsync();

                return new JsonResult(new
                {
                    Event = eventDetails,
                    Customer = customerDetails
                });
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        public async Task<EventDetails> EditEvent(int eventId)
        {
            try
            {
                
                Event eventDetails = await _context.Events
                    .Where(e => e.EventId == eventId)
                    .FirstOrDefaultAsync();

                Customer customerDetails = await _context.Customers
                    .Where(c => c.CustomerId == eventDetails.CustomerId)
                    .FirstOrDefaultAsync();

                EventDetails model = new EventDetails()
                {
                    EventId = eventDetails.EventId,
                    Start = eventDetails.Start,
                    End = eventDetails.End,
                    Title = eventDetails.Title,
                    Color = eventDetails.Color,
                    Notes = eventDetails.Notes,

                    CustomerId = customerDetails.CustomerId,
                    Name = customerDetails.Name,
                    Surname = customerDetails.Surname,
                    Mobile = customerDetails.Mobile,
                    Email = customerDetails.Email,
                    LastVisitDate = customerDetails.LastVisitDate
                };

                return model;
            
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }


        public async Task<JsonResult> SearchCustomers(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new JsonResult(new List<object>());
            }

            var customers = await _context.Customers
                .Where(c => EF.Functions.Like(c.Name, $"%{text}%") || EF.Functions.Like(c.Surname, $"%{text}%"))
                .Select(c => new
                {
                    Id = c.CustomerId,
                    Name = c.Name,
                    Surname = c.Surname
                })
                .Take(10) // massimo 10 risultati per evitare di caricare troppo
                .ToListAsync();

            return new JsonResult(customers);
        }

        public async Task<JsonResult> GetCustomerById(int id)
        {
            if (id == 0)
            {
                return new JsonResult(new object());
            }

            var customer = await _context.Customers
                .Where(c => c.CustomerId == id)
                .Select(c => new
                {
                    Id = c.CustomerId,
                    Name = c.Name,
                    Surname = c.Surname,
                    Email = c.Email,
                    Mobile = c.Mobile
                })
                .FirstOrDefaultAsync();
    
            return new JsonResult(customer);
        }

        public async Task<EventDetails> UpdateEvent(EventDetails dto)
        {
            try
            {
                var eventToUpdate = await _context.Events
                    .FirstOrDefaultAsync(e => e.EventId == dto.EventId);

                if (eventToUpdate == null)
                    return null;

                eventToUpdate.Title = dto.Title;
                eventToUpdate.Start = dto.Start;
                eventToUpdate.End = dto.End;
                eventToUpdate.Notes = dto.Notes;

                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }
        
        public async Task<bool> DeleteEvent(int eventId)
        {
            try
            {
                var eventToDelete = await _context.Events
                    .FirstOrDefaultAsync(e => e.EventId == eventId);

                if (eventToDelete == null)
                    return false;

                _context.Events.Remove(eventToDelete);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

    }
}
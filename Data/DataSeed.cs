using ccalendar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ccalendar.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (context.Customers.Any())
                return; // Già popolato

            var customers = new List<Customer>();
            var random = new Random();

            for (int i = 1; i <= 10; i++)
            {
                var customer = new Customer
                {
                    Name = $"Nome{i}",
                    Surname = $"Cognome{i}",
                    Mobile = $"32000000{i:D2}",
                    Email = $"cliente{i}@mail.com",
                    ToCall = i % 2 == 0,
                    Active = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-i),
                    LastVisitDate = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                };

                var start = DateTime.Today.AddHours(9 + i);
                var end = start.AddHours(1);

                var evt = new Event
                {
                    Title = $"Evento Cliente {i}",
                    Start = start,
                    End = end,
                    AllDay = false,
                    Color = "#3788d8",
                    IsVisit = true,
                    EventType = "Visita",
                    Customer = customer
                };

                customer.Events.Add(evt);
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}

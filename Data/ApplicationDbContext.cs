using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ccalendar.Models;
using Microsoft.EntityFrameworkCore;

namespace ccalendar.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Event> Events { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    }
}
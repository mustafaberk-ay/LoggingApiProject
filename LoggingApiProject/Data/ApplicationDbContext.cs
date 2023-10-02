using LoggingApiProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LoggingApiProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<LogMessage> Logs { get; set; }
    }
}

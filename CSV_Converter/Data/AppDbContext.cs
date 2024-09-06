using CSV_Converter.Models;
using Microsoft.EntityFrameworkCore;

namespace CSV_Converter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }

        public DbSet<Order> Orders { get; set; }
    }
}

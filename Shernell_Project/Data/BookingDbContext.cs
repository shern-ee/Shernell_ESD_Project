using Microsoft.EntityFrameworkCore;
using Shernell_Project.Models;

namespace Shernell_Project.Data
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options)
            : base(options) 
        { 
        }

        public DbSet<Facility>? Facilities { get; set; }
    }
}

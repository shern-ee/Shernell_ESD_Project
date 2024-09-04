using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shernell_Project.Models;

namespace Shernell_Project.Data
{
    public class BookingDbContext : IdentityDbContext<IdentityUser>
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options)
            : base(options) 
        { 
        }

        public DbSet<Facility>? Facilities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

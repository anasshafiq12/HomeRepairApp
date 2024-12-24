using HouseRepairApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseRepairApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<MyUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensures Identity tables are set up correctly

            modelBuilder.Entity<Order>()
                .HasMany(o => o.cartItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Booking> Bookings { get; set; } // table
        public DbSet<CartItem> CartItems { get; set; } // table
        public DbSet<Order> Orders { get; set; } // table
    }
}

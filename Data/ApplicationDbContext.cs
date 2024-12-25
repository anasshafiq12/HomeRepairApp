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
            modelBuilder.Entity<Cart>()
                .HasOne(cart => cart.Order)
                .WithOne(order => order.Cart)
                .HasForeignKey<Cart>(cart => cart.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }



        public DbSet<Booking> Bookings { get; set; } // table
        public DbSet<CartItem> CartItems { get; set; } // table
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; } // table
    }
}

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
            //modelBuilder.Entity<CartItem>()
            //    .HasOne(a => a.Cart)
            //    .WithMany(a => a.CartItems)
            //    .HasForeignKey(a => a.CartId)
            //    .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }



        public DbSet<Booking> Bookings { get; set; } // table
        public DbSet<CartItem> CartItems { get; set; } // table
        public DbSet<Cart> Carts { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Mottu.Application.Common.Interfaces;
using Mottu.Domain.Entities;
using Mottu.Infrastructure.Configurations;

namespace Mottu.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Deliveryman> Deliverymans { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Notification> Notifications { get ; set; }
        public DbSet<Order> Orders { get ; set; }

    // public DbSet<DeliveryPerson> DeliveryPersons { get; set; }
    // public DbSet<Rental> Rentals { get; set; }
    // public DbSet<Order> Orders { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    { 
      
    }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MotorcycleConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DeliverymanConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }
  }
}

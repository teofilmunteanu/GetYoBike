using GetYoBike.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace GetYoBike.Server.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<BikeType> BikeTypes { get; set; }
        public DbSet<Rent> Rents { get; set; }


        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rent>().HasKey(r => new { r.UserID, r.BikeID });

            modelBuilder.Entity<Bike>().HasMany(b => b.Rents).WithOne(r => r.RentedBike);
            modelBuilder.Entity<User>().HasMany(b => b.Rents).WithOne(u => u.RenterUser);
            modelBuilder.Entity<BikeType>().HasMany(r => r.Bikes).WithOne(b => b.Type);
        }

    }
}

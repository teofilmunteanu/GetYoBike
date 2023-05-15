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
            modelBuilder.Entity<Bike>().HasMany(b => b.Rents).WithOne(r => r.RentedBike).HasForeignKey(r => r.RentedBikeId);
            modelBuilder.Entity<User>().HasMany(b => b.Rents).WithOne(u => u.RenterUser).HasForeignKey(u => u.RenterUserId);
            modelBuilder.Entity<BikeType>().HasMany(t => t.Bikes).WithOne(b => b.Type).HasForeignKey(t => t.TypeId);
        }
    }
}

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
            modelBuilder.Entity<Bike>().HasMany(b => b.Rents).WithOne(r => r.RentedBike).OnDelete(DeleteBehavior.Cascade); ;
            modelBuilder.Entity<User>().HasMany(b => b.Rents).WithOne(u => u.RenterUser).OnDelete(DeleteBehavior.Cascade); ;
            modelBuilder.Entity<BikeType>().HasMany(r => r.Bikes).WithOne(b => b.Type).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bike>().Navigation(e => e.Type).AutoInclude();
            modelBuilder.Entity<Rent>().Navigation(e => e.RentedBike).AutoInclude();
            modelBuilder.Entity<Rent>().Navigation(e => e.RenterUser).AutoInclude();
        }

    }
}

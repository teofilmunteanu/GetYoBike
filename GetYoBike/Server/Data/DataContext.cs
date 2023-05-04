using GetYoBike.Server.Models;
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

            modelBuilder.Entity<BikeType>().HasData(
                new BikeType(1,5,Types.city),
                new BikeType(2,10,Types.mountain)
            );
        }

    }
}

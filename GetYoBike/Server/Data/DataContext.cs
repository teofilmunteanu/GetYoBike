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


            modelBuilder.Entity<BikeType>().HasData(
                new BikeType()
                {
                    Id = 1,
                    Price = 5,
                    Type = Types.city
                },
                new BikeType()
                {
                    Id = 2,
                    Price = 10,
                    Type = Types.mountain
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Email = "test",
                    LastName = "test",
                    FirstName = "test",
                    Age = 5
                }
            );
        }

    }
}

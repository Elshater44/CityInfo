using CityInfo.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CityInfo.Api.DbContexts
{
    public class CityInfoDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options)
            : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(
                new City("New York City")
                {
                    Id = 1,
                    Description = "The one with big park"
                },
                new City("Antwarp")
                {
                    Id = 2,
                    Description = "The one with big cathedral that was never finished"
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "The one with big tower"
                });
            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                new PointOfInterest("Statue of Liberty")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "A symbol of freedom"
                },
                new PointOfInterest("Central Park")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "An iconic urban park"
                },
                new PointOfInterest("Cathedral of Our Lady")
                {
                    Id = 3,
                    CityId = 2,
                    Description = "An impressive Gothic cathedral"
                },
                new PointOfInterest("Antwerp Zoo")
                {
                    Id = 4,
                    CityId = 2,
                    Description = "One of the oldest zoos in the world"
                },
                new PointOfInterest("Eiffel Tower")
                {
                    Id = 5,
                    CityId = 3,
                    Description = "Iconic iron lattice tower"
                },
                new PointOfInterest("Louvre Museum")
                {
                    Id = 6,
                    CityId = 3,
                    Description = "World's largest art museum"
                });
            base.OnModelCreating(modelBuilder); 
        }
    }
}

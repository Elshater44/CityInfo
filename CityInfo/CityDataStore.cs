using CityInfo.Api.Models;

namespace CityInfo.Api
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        //public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public CitiesDataStore()
        {
            this.Cities = new List<CityDto>()
            {
                new CityDto
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with the big park",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id = 1,
                            Name = "Statue of Liberty",
                            Description = "A symbol of freedom"
                        },
                        new PointOfInterestDto
                        {
                            Id = 2,
                            Name = "Central Park",
                            Description = "An iconic urban park"
                        }
                    }
                },
                new CityDto
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the big cathedral that was never really finished.",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id = 1,
                            Name = "Cathedral of Our Lady",
                            Description = "An impressive Gothic cathedral"
                        },
                        new PointOfInterestDto
                        {
                            Id = 2,
                            Name = "Antwerp Zoo",
                            Description = "One of the oldest zoos in the world"
                        }
                    }
                },
                new CityDto
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower",
                    PointsOfInterest = new List<PointOfInterestDto>
                    {
                        new PointOfInterestDto
                        {
                            Id = 1,
                            Name = "Eiffel Tower",
                            Description = "Iconic iron lattice tower"
                        },
                        new PointOfInterestDto
                        {
                            Id = 2,
                            Name = "Louvre Museum",
                            Description = "World's largest art museum"
                        }
                    }
                }
            };
        }
    }
}

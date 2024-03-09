using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;
using CityInfo.Models;

namespace CityInfo.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityWithoutPointOfInterestDto>();
            CreateMap<City, CityDto>();

        }
    }
}

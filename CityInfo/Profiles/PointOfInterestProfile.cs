using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;

namespace CityInfo.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<PointOfInterest, PointOfInterestDto>();
            CreateMap<PointOfInterestForCreationDto, PointOfInterest>();
            CreateMap<PointOfInterestForUpdating, PointOfInterest>();
            CreateMap<PointOfInterest, PointOfInterestForUpdating>();
        }
    }
}

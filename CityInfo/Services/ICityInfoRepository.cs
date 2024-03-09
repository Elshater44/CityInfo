using CityInfo.Api.Entities;

namespace CityInfo.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int cityId,bool includePointOfInterests);
        Task<bool> CityExistsAsync(int cityId);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);
        Task<PointOfInterest> GetPointOfInterestForCityAsync(int pointOfInterestId, int cityId);
        Task AddPointOfInterestForCityAsync(int cityId,PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<(IEnumerable<City>, PaginationMetaData)> GetCitiesAsync(string? name,string? searchQuery,int pageNumber,int pageSize);
        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
        Task<bool> SaveChangesAsync();

    }
}

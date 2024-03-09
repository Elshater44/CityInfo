using AutoMapper;
using CityInfo.Api.Models;
using CityInfo.Models;
using CityInfo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        const int maxCitiesPageSize = 20;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ;
            _mapper = mapper ?? throw new NullReferenceException(nameof(mapper));
        }
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> 
            GetCities(string? name,string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if(pageSize > maxCitiesPageSize)
            {
                pageSize = maxCitiesPageSize;
            }

            var (cityEntities, paginationMetaData) = await _cityInfoRepository.GetCitiesAsync(name,searchQuery,pageNumber,pageSize);
            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(paginationMetaData));
            var item = _mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities);
            return Ok(item);
        }
        /// <summary>
        /// Get a city by Id
        /// </summary>
        /// <param name="id">The id of city to get</param>
        /// <param name="includePointsOfInterest">Whether or not to include all points of interest or not at all</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">returns the requested cities</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCity(int id, bool includePointsOfInterest)
        {
            var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
            if (city == null) return NotFound();
            if (includePointsOfInterest) return Ok(_mapper.Map<CityDto>(city));
            return Ok(_mapper.Map<CityWithoutPointOfInterestDto>(city));
        }
    }
}

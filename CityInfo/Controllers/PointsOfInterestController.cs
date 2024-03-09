using AutoMapper;
using CityInfo.Api.Entities;
using CityInfo.Api.Models;
using CityInfo.Api.Services;
using CityInfo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.Api.Controllers
{
    [ApiController]
    [Authorize(Policy = "MustBeFromAntwerp")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities/{cityId}/[controller]")]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger , 
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetAllPointsOfInterest(int cityId)
        {
            var cityName = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value;
            if (!await _cityInfoRepository.CityNameMatchesCityId(cityName, cityId)) return Forbid();
            try
            {
                //throw new Exception("Exception sample
                if (!await _cityInfoRepository.CityExistsAsync(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }
                var pointsOfInterest = await _cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);
                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterest));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(
                    $"Exception while getting points of interest for city with id {cityId}."
                    ,ex);
                return StatusCode(500, "A problem has occured while handling your request.");
            }
        }

        [HttpGet("{pointId}" , Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetAllPointsOfInterest(int cityId , int pointId)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = await _cityInfoRepository.GetPointOfInterestForCityAsync(pointId, cityId);

            
            if (pointOfInterest == null)
                return NotFound();

            return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));
        }

        [HttpPost()]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId , PointOfInterestForCreationDto pointOfInterestForCreation)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterestForCreation);
            await _cityInfoRepository.AddPointOfInterestForCityAsync(cityId, finalPointOfInterest);
            await _cityInfoRepository.SaveChangesAsync();

            var createdPointOfInterestToReturn = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointId = createdPointOfInterestToReturn.Id
                }, createdPointOfInterestToReturn);
        }

        [HttpPut("{pointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterestAsync(int cityId,int pointOfInterestId , PointOfInterestForUpdating pointOfInterestForUpdating)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound("City Not Found");

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(pointOfInterestId, cityId);


            if (pointOfInterestEntity == null) return NotFound("PointOfInterest Not Found");

            _mapper.Map(pointOfInterestForUpdating,pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }
        [HttpPatch("{pointOfInterestId}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdating> patchDocument)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();

            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(pointOfInterestId,cityId);

            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdating>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(pointOfInterestToPatch))
                return BadRequest(ModelState);
            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{pointOfInterestId}")]
        public async Task<ActionResult>DeletePointOfInterest(int cityId , int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId)) return NotFound();
            var pointOfInterestEntity = await _cityInfoRepository.GetPointOfInterestForCityAsync(pointOfInterestId,cityId);
            
            if (pointOfInterestEntity == null) return NotFound();

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);
            await _cityInfoRepository.SaveChangesAsync();
            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} has been deleted");
            return NoContent();
        }
    }
}

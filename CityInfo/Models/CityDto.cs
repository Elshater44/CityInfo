namespace CityInfo.Api.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int NumberPointOfInterest 
        {   
            get { return this.PointsOfInterest.Count; } 
        }
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();

    }
}

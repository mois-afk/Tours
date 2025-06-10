namespace Tours.Models
{
    public class AttractionModel
    {
        public string? AttractionId { get; set; }

        public string? AttractionName { get; set; }

        public string? AttractionDescription { get; set; }

        public string? AttractionPhotoUrl { get; set; }

        public string? CityId { get; set; }

        public List<City>? Cities { get; set; }
    }
}

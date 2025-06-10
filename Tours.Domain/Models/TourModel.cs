namespace Tours.Models
{
    public class TourModel
    {
        public string? TourId { get; set; }

        public string? TourName { get; set; }

        public string? TourDescription { get; set; }

        public string? URL { get; set; }

        public double TourPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Dictionary<string, DateTime>? AttractionDate { get; set; }

        public List<Attraction>? Attractions { get; set; }
    }
}

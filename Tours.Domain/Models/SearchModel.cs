namespace Tours.Models
{
    using Tours;

    public class SearchModel
    {
        public string? Query { get; set; }

        public List<City>? Cities { get; set; }

        public List<Attraction>? Attractions { get; set; }

        public List<Tour>? Tours { get; set; }

        public bool ShowTours { get; set; }

        public bool ShowAttractions { get; set; }

        public bool ShowCities { get; set; }
    }
}

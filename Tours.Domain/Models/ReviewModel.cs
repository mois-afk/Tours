namespace Tours.Models
{
    public class ReviewModel
    {
        public string? Username { get; set; }

        public string? ReviewText { get; set; }

        public Attraction? Attraction { get; set; }

        public Tour? Tour { get; set; }
    }
}

namespace Tours
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Tour
    {
        public Tour(string name, string description, double price, DateTime startDate, DateTime endDate, Dictionary<string, DateTime> attractionDate, string url, bool userGenerated = false)
        {
            TourName = name;
            TourDescription = description;
            TourPrice = price;
            StartDate = startDate;
            EndDate = endDate;
            AttractionDate = attractionDate;
            URL = url;
            UserGeneratedTour = userGenerated;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TourId { get; set; }

        public string TourName { get; set; }

        public string TourDescription { get; set; }

        public string URL { get; set; }

        public double TourPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Dictionary<string, DateTime> AttractionDate { get; set; }
        public bool? UserGeneratedTour { get; set; } = false;
    }
}

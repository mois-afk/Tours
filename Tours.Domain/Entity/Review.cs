namespace Tours
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Review
    {
        public Review(string username, string reviewText, string tourId)
        {
            Username = username;
            ReviewText = reviewText;
            TourId = tourId;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReviewId { get; set; }

        public string Username { get; set; }

        public string ReviewText { get; set; }

        public string TourId { get; set; }
    }
}

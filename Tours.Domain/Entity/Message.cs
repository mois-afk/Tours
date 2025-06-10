namespace Tours
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Message
    {
        public Message(string username, string text, DateTime date)
        {
            Username = username;
            Text = text;
            Like = new HashSet<string>();
            Dislike = new HashSet<string>();
            Comments = new List<Message>();
            Date = date;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageId { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public HashSet<string> Like { get; set; }

        public HashSet<string> Dislike { get; set; }

        public List<Message> Comments { get; set; }

        public DateTime Date { get; set; }
    }
}

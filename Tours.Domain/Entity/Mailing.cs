namespace Tours
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Mailing
    {
        public Mailing(string subject, string body, DateTime date)
        {
            EmailList = new List<string>();
            Subject = subject;
            Body = body;
            Date = date;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MailingId { get; set; }

        public List<string> EmailList { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime Date { get; set; }
    }
}

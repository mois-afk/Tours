namespace Tours
{
    using System;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Email
    {
        public Email(string emailFrom, string text, DateTime date)
        {
            EmailFrom = emailFrom;
            Text = text;
            Date = date;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EmailId { get; set; }

        public string EmailFrom { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}

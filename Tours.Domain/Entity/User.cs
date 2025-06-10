namespace Tours
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class User
    {
        public User(string username, string email, string password, bool isAdmin)
        {
            Username = username;
            Email = email;
            Password = password;
            IsAdmin = isAdmin;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public string Username { get; set; }

        public string PhotoUrl { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}

namespace PIS.Memory
{
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using Tours;
    using Tours.Interface;
    using Tours.Models;

    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>("Users");
        }

        public async Task AddUserAsync(string email, string username, string password)
        {
            User user = new User(email, username, password, false);
            await _userCollection.InsertOneAsync(user);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _userCollection.Find(user => user.UserId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUser(string userId, string email, string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserId, userId);
            var update = Builders<User>.Update
                .Set(u => u.Username, username)
                .Set(u => u.Email, email);

            var updateResult = await _userCollection.UpdateOneAsync(filter, update);
            return updateResult.ModifiedCount > 0;
        }

        public async Task<bool> UpdatePassword(string password, string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            var update = Builders<User>.Update
                .Set(u => u.Password, password);

            var updateResult = await _userCollection.UpdateOneAsync(filter, update);
            return updateResult.ModifiedCount > 0;
        }

        public async Task DeleteUserAsync(string id)
        {
            await _userCollection.DeleteOneAsync(user => user.UserId == id);
        }

        public async Task<User> GetUserByUsernameAsync(string userName)
        {
            return await _userCollection.Find(user => user.Username == userName).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> UsernameNotExistAsync(string userName)
        {
            var newUser = await _userCollection.Find(user => user.Username == userName).FirstOrDefaultAsync();
            return newUser == null;
        }

        public async Task<bool> UserEmailNotExistAsync(string email)
        {
            var newUser = await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
            return newUser == null;
        }

        public async Task<string> GetUsernameByEmailAsync(string email)
        {
            var user = await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
            return user?.Username;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<User>> GetUsersByPartUsernameAsync(string partName)
        {
            return await _userCollection.Find(user => user.Username.ToLower().Contains(partName.ToLower())).ToListAsync();

        }
    }
}

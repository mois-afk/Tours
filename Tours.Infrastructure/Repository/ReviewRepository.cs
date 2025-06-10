namespace PIS.Memory
{
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tours;
    using Tours.Interface;
    using Tours.Models;

    namespace PIS.Memory
    {
        public class ReviewRepository : IReviewRepository
        {
            private readonly IMongoCollection<Review> _reviewCollection;

            public ReviewRepository(IOptions<MongoDbSettings> mongoDbSettings)
            {
                var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
                var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
                _reviewCollection = database.GetCollection<Review>("Reviews");
            }

            public async Task<List<Review>> GetAllReview()
            {
                return await _reviewCollection.Find(_ => true).ToListAsync();
            }

            public async Task AddReview(Review review)
            {
                await _reviewCollection.InsertOneAsync(review);
            }

            public async Task<List<Review>> GetAllReviewsByUsername(string username)
            {
                return await _reviewCollection.Find(review => review.Username.Equals(username)).ToListAsync();
            }
        }
    }

}

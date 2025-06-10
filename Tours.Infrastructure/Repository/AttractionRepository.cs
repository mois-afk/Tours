namespace PIS.Memory
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using Tours;
    using Tours.Interface;
    using Tours.Models;

    public class AttractionRepository : IAttractionRepository
    {
        private readonly IMongoCollection<Attraction> _attractionCollection;

        public AttractionRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _attractionCollection = database.GetCollection<Attraction>("Attractions");
        }

        public async Task<List<Attraction>> GetAllAttractions()
        {
            return await _attractionCollection.Find(attraction => true).ToListAsync();
        }

        public async Task<bool> UpdateAttraction(AttractionModel model)
        {
            var filter = Builders<Attraction>.Filter.Eq(a => a.AttractionId, model.AttractionId);
            var update = Builders<Attraction>.Update
                .Set(a => a.AttractionName, model.AttractionName)
                .Set(a => a.AttractionDescription, model.AttractionDescription)
                .Set(a => a.AttractionPhotoUrl, model.AttractionPhotoUrl)
                .Set(a => a.CityId, model.CityId);

            var updateResult = await _attractionCollection.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }

        public async Task<List<Attraction>> GetAllAttractionsByCityId(string cityId)
        {
            return await _attractionCollection.Find(attraction => attraction.CityId == cityId).ToListAsync();
        }

        public async Task<List<Attraction>> GetAllAttractionsByName(string namePart)
        {
            return await _attractionCollection.Find(attraction => attraction.AttractionName.ToLower().Contains(namePart.ToLower())).ToListAsync();
        }

        public async Task AddAttraction(string name, string description, string URL, string cityId)
        {
            var attraction = new Attraction(name, description, cityId, URL);
            await _attractionCollection.InsertOneAsync(attraction);
        }

        public async Task<Attraction> GetAttractionById(string id)
        {
            return await _attractionCollection.Find(attraction => attraction.AttractionId == id).FirstOrDefaultAsync();
        }

        public async Task DeleteAttractionById(string id)
        {
            await _attractionCollection.DeleteOneAsync(attraction => attraction.AttractionId == id);
        }
    }
}

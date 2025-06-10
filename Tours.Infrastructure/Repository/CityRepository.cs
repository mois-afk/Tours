namespace PIS.Memory
{
    using MongoDB.Driver;
    using Microsoft.Extensions.Options;
    using Tours.Models;
    using Tours.Interface;
    using Tours;

    public class CityRepository : ICityRepository
    {
        private readonly IMongoCollection<City> _cityCollection;

        public CityRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _cityCollection = database.GetCollection<City>("Cities");
        }

        public async Task<List<City>> GetAllCity()
        {
            return await _cityCollection.Find(city => true).ToListAsync();
        }

        public async Task<List<City>> GetAllCityByName(string namePart)
        {
            return await _cityCollection.Find(city => city.CityName.ToLower().Contains(namePart.ToLower())).ToListAsync();
        }

        public async Task AddCity(string Url, string name, string descriprion)
        {
            var city = new City(name, descriprion, Url);

            await _cityCollection.InsertOneAsync(city);
        }

        public async Task<bool> UpdateCity (CityModel model)
        {
            var filter = Builders<City>.Filter.Eq(c => c.CityId, model.CityId);
            var update = Builders<City>.Update
                .Set(c => c.CityName, model.CityName)
                .Set(c => c.CityDescription, model.CityDescription)
                .Set(c => c.PhotoUrl, model.URL);

            var updateResult = await _cityCollection.UpdateOneAsync(filter, update);

            return updateResult.ModifiedCount > 0;
        }

        public async Task<City> GetCityById(string id)
        {
            return await _cityCollection.Find(city => city.CityId == id).FirstOrDefaultAsync();
        }

        public async Task DeleteCityById(string id)
        {
            await _cityCollection.DeleteOneAsync(city => city.CityId == id);
        }
    }
}

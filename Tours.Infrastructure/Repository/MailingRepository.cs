namespace PIS.Memory
{
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using Tours;
    using Tours.Interface;
    using Tours.Models;

    public class MailingRepository : IMailingRepository
    {
        private readonly IMongoCollection<Mailing> _mailingCollection;

        public MailingRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _mailingCollection = database.GetCollection<Mailing>("Mailing");
        }

        public async Task AddMailing(Mailing mailing)
        {
           await _mailingCollection.InsertOneAsync(mailing);
        }

        public async Task<List<Mailing>> GetAllMailing()
        {
            return await _mailingCollection.Find(_ => true).ToListAsync();
        }

        public async Task<List<string>> GetEmailList(string id)
        {
            var emailList = new List<string>();

            var mailing = await _mailingCollection.Find(m => m.MailingId == id).FirstOrDefaultAsync();

            if (mailing != null)
            {
                emailList.AddRange(mailing.EmailList);
            }

            return emailList;
        }

        public async Task<bool> AddToEmailList(string email, string id)
        {
            var filter = Builders<Mailing>.Filter.Eq(m => m.MailingId, id);
            var update = Builders<Mailing>.Update.Push(m => m.EmailList, email);

            var result = await _mailingCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteFromEmailList(string email, string id)
        {
            var filter = Builders<Mailing>.Filter.Eq(m => m.MailingId, id);
            var update = Builders<Mailing>.Update.Pull(m => m.EmailList, email);

            var result = await _mailingCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }

        public async Task<Mailing> GetMailingById(string id)
        {
            return await _mailingCollection.Find(m => m.MailingId == id).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteMailing(string id)
        {
            var filter = Builders<Mailing>.Filter.Eq(m => m.MailingId, id);
            var result = await _mailingCollection.DeleteOneAsync(filter);

            return result.DeletedCount > 0;
        }
    }
}

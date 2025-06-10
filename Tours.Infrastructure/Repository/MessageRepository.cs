using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Tours;
using Tours.Models;

namespace PIS.Memory
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<Message> _messageCollection;

        public MessageRepository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var client = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _messageCollection = database.GetCollection<Message>("Chat");
        }

        public async Task AddMessage(Message message)
        {
            await _messageCollection.InsertOneAsync(message);
        }

        public async Task<List<Message>> GetAllMessages()
        {
            return await _messageCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Message> GetMessageById(string id)
        {
            return await _messageCollection.Find(m => m.MessageId == id).FirstOrDefaultAsync();
        }

        public async Task AddLikeToMessage(string messageId, string username)
        {
            var update = Builders<Message>.Update.AddToSet(m => m.Like, username);
            await _messageCollection.UpdateOneAsync(m => m.MessageId == messageId, update);
        }

        public async Task AddDislikeToMessage(string messageId, string username)
        {
            var update = Builders<Message>.Update.AddToSet(m => m.Dislike, username);
            await _messageCollection.UpdateOneAsync(m => m.MessageId == messageId, update);
        }

        public async Task AddCommentToMessage(string messageId, Message comment)
        {
            var update = Builders<Message>.Update.Push(m => m.Comments, comment);
            await _messageCollection.UpdateOneAsync(m => m.MessageId == messageId, update);
        }

        public async Task<bool> DeleteMessage(string id)
        {
            var result = await _messageCollection.DeleteOneAsync(m => m.MessageId == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task RemoveLikeFromMessage(string messageId, string username)
        {
            var update = Builders<Message>.Update.Pull(m => m.Like, username);
            await _messageCollection.UpdateOneAsync(m => m.MessageId == messageId, update);
        }

        public async Task RemoveDislikeFromMessage(string messageId, string username)
        {
            var update = Builders<Message>.Update.Pull(m => m.Dislike, username);
            await _messageCollection.UpdateOneAsync(m => m.MessageId == messageId, update);
        }

        public async Task RemoveCommentFromMessage(string messageId, Message comment)
        {
            var update = Builders<Message>.Update.Pull(m => m.Comments, comment);
            await _messageCollection.UpdateOneAsync(m => m.MessageId == messageId, update);
        }
    }
}

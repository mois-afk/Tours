namespace Tours
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tours.Models;

    public interface IMessageRepository
    {
        public Task AddMessage(Message message);

        public Task<List<Message>> GetAllMessages();

        public Task<Message> GetMessageById(string id);

        public Task AddLikeToMessage(string messageId, string username);

        public Task AddDislikeToMessage(string messageId, string username);

        public Task AddCommentToMessage(string messageId, Message comment);

        public Task<bool> DeleteMessage(string id);

        public Task RemoveLikeFromMessage(string messageId, string username);

        public Task RemoveDislikeFromMessage(string messageId, string username);

        public Task RemoveCommentFromMessage(string messageId, Message comment);
    }
}

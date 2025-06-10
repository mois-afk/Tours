using Tours;

namespace Tours.Service.Interface
{
    public interface IChatService
    {
        public Task<List<Message>> GetAllMessage();

        public Task AddMessage(string text, string username);

        public Task LikeMessage(string id, string username);

        public Task DislikeMessage(string id, string username);

        public Task UnLikeMessage(string id, string username);

        public Task UnDislikeMessage(string id, string username);

        public Task AddComment(string id, string text, string username);
    }
}

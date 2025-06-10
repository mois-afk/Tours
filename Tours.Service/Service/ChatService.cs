namespace Tours.Service
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Tours.Service.Interface;

    public class ChatService : IChatService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IRedisService _redisService;

        public ChatService(IMessageRepository messageRepository, IRedisService redisService)
        {
            _messageRepository = messageRepository;
            _redisService = redisService;
        }

        public async Task<List<Message>> GetAllMessage()
        {
            return await _messageRepository.GetAllMessages();
        }

        public async Task AddMessage(string text, string username)
        {
            Message message = new Message(username, text, DateTime.Now);
            await _messageRepository.AddMessage(message);
        }

        public async Task LikeMessage(string id, string username)
        {
            await _messageRepository.AddLikeToMessage(id, username);
        }

        public async Task DislikeMessage(string id, string username)
        {
            await _messageRepository.AddDislikeToMessage(id, username);
        }

        public async Task UnLikeMessage(string id, string username)
        {
            await _messageRepository.RemoveLikeFromMessage(id, username);
        }

        public async Task UnDislikeMessage(string id, string username)
        {
            await _messageRepository.RemoveDislikeFromMessage(id, username);
        }

        public async Task AddComment(string id, string text, string username)
        {
            Message message = new Message(username, text, DateTime.Now);
            await _messageRepository.AddCommentToMessage(id, message);
        }
    }
}

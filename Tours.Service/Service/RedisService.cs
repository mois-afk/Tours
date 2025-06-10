using System.Text.Json;
using StackExchange.Redis;
using Tours.Service.Interface;

namespace Tours
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _redisDatabase;

        public RedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public T Get<T>(string key)
        {
            var value = _redisDatabase.StringGet(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public void Set<T>(string key, T value)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            _redisDatabase.StringSet(key, serializedValue);
        }

        public void Remove(string key)
        {
            _redisDatabase.KeyDelete(key);
        }
    }
}
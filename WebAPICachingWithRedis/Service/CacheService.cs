using StackExchange.Redis;
using System.Text.Json;

namespace WebAPICachingWithRedis.Service
{
    public class CacheService : ICacheService
    {
        private IDatabase _cachedb;
        public CacheService()
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            _cachedb = redis.GetDatabase();
        }
        public T GetData<T>(string key)
        {
            var value = _cachedb.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }
            return default;
        }

        public object RemoveData(string key)
        {
            var _exist=_cachedb.KeyExists(key);
            if (_exist)
            {
                return _cachedb.KeyDelete(key);
            }
            return false;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiretyTime = expirationTime.DateTime.Subtract(DateTime.Now);
            return _cachedb.StringSet(key, JsonSerializer.Serialize(value), expiretyTime);
        }
    }
}

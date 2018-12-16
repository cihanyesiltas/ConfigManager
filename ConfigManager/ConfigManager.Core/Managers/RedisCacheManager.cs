using System;
using ConfigManager.Core.Contracts;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ConfigManager.Core.Managers
{
    public class RedisCacheManager:ICacheManager
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect("localhost"));

        private static ConnectionMultiplexer Connection => LazyConnection.Value;

        public T Get<T>(string key)
        {
            var cacheDb = Connection.GetDatabase();

            var value = cacheDb.StringGet(key);

            if (!value.IsNullOrEmpty)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default(T);
        }

        public void Remove(string key)
        {
            var cacheDb = Connection.GetDatabase();
            cacheDb.KeyDelete(key);
        }

        public void Add<T>(string key, T value)
        {
            var cacheDb = Connection.GetDatabase();

            var serializedObject = JsonConvert.SerializeObject(value);

            cacheDb.StringSet(key, serializedObject);
        }
    }
}

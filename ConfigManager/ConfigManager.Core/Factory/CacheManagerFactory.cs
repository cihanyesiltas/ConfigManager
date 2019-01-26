using System;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DataProviders.MongoDbProvider;
using ConfigManager.Core.DataProviders.PostgreSqlProvider;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Enums;
using ConfigManager.Core.Managers;

namespace ConfigManager.Core.Implementations
{
    public class CacheManagerFactory : ICacheManagerFactory
    {
        public IStorageProvider Create(Connection connection)
        {
            switch (connection.StorageProviderType)
            {
                case StorageProviderType.MongoDb:
                    return new MongoDbStorageProvider(connection.ConnectionString);
                case StorageProviderType.PostgreSQL:
                    return new PostgreSqlStorageProvider(connection.ConnectionString);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ICacheManager Create(CacheManagerType type)
        {
            switch (type)
            {
                case CacheManagerType.MemoryCache:
                    return new MemoryCacheManager();
                case CacheManagerType.RedisCache:
                    return new RedisCacheManager();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}

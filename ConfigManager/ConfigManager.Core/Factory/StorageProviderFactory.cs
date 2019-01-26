using System;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DataProviders.InMemoryDbProvider;
using ConfigManager.Core.DataProviders.MongoDbProvider;
using ConfigManager.Core.DataProviders.PostgreSqlProvider;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Enums;

namespace ConfigManager.Core.Implementations
{
    public class StorageProviderFactory : IStorageProviderFactory
    {
        public IStorageProvider Create(Connection connection)
        {
            switch (connection.StorageProviderType)
            {
                case StorageProviderType.MongoDb:
                    return new MongoDbStorageProvider(connection.ConnectionString);
                case StorageProviderType.PostgreSQL:
                    return new PostgreSqlStorageProvider(connection.ConnectionString);
                case StorageProviderType.InMemoryDb:
                    return new InMemoryDbStorageProvider();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

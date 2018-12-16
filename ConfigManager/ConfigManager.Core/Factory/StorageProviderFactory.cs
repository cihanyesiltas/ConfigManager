using System;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Enums;
using ConfigManager.Core.Providers.MongoDbProvider;
using ConfigManager.Core.Providers.PostgreSqlProvider;

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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

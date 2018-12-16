using System;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Implementations;
using ConfigManager.Core.Managers;

namespace ConfigManager.Core.Factory
{
    public class ConfigurationReaderFactory : IConfigurationReaderFactory
    {
        private readonly IStorageProviderFactory _storageProviderFactory;

        public ConfigurationReaderFactory()
        {
            _storageProviderFactory = new StorageProviderFactory();
        }

        public IConfigurationReader Create(string applicationName, Connection connection, int refreshTimerIntervalInMs)
        {
            var storageProvider = _storageProviderFactory.Create(connection);

            return new ConfigurationReader(new RedisCacheManager(), storageProvider, applicationName, refreshTimerIntervalInMs);
        }
    }
}

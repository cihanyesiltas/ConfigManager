using System;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Enums;
using ConfigManager.Core.Implementations;
using ConfigManager.Core.Managers;

namespace ConfigManager.Core.Factory
{
    public class ConfigurationReaderFactory : IConfigurationReaderFactory
    {
        private readonly IStorageProviderFactory _storageProviderFactory;
        private readonly ICacheManagerFactory _cacheManagerFactory;
        
        public ConfigurationReaderFactory()
        {
            _storageProviderFactory = new StorageProviderFactory();
            _cacheManagerFactory = new CacheManagerFactory();
        }

        public IConfigurationReader Create(string applicationName, Connection connection, int refreshTimerIntervalInMs)
        {
            var storageProvider = _storageProviderFactory.Create(connection);
            var cacheManager = _cacheManagerFactory.Create(CacheManagerType.MemoryCache);
            
            return new ConfigurationReader(cacheManager, storageProvider, applicationName, refreshTimerIntervalInMs);
        }
    }
}

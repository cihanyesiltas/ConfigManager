using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConfigManager.Core.Const;
using ConfigManager.Core.Contracts;
using ConfigManager.Core.DTOs;
using ConfigManager.Core.Extensions;
using ConfigManager.Core.Validators;

namespace ConfigManager.Core.Managers
{
    public class ConfigurationReader : IConfigurationReader, IDisposable
    {
        private readonly IStorageProvider _storageProvider;
        private readonly ICacheManager _cacheManager;
        private readonly string _applicationName;
        private readonly int _refreshTimerIntervalInMs;
        private System.Timers.Timer _timer;

        public ConfigurationReader(ICacheManager cacheManager, IStorageProvider storageProvider, string applicationName, int refreshTimerIntervalInMs)
        {
            _storageProvider = storageProvider;
            _applicationName = applicationName;
            _refreshTimerIntervalInMs = refreshTimerIntervalInMs;
            _cacheManager = cacheManager;
            InitiliazeRefreshCacheTimer();
        }

        private List<CacheConfigurationDTO> CachedList => _cacheManager.Get<List<CacheConfigurationDTO>>(string.Format(CacheKey.ApplicationConfigurationParameterList));

        public T GetValue<T>(string key)
        {
            var cachedItem = CachedList.FirstOrDefault(a => a.Name == key && a.IsActive);

            if (cachedItem != null)
            {
                return cachedItem.Value.Cast<T>(cachedItem.Type);
            }
            return default(T);
        }

        public bool Add(AddConfigurationDTO dto)
        {
            var validationResult = new AddNewConfigurationValidator().Validate(dto);
            if (validationResult.IsValid)
            {
                var isExist = _storageProvider.Exists(dto.Name, _applicationName);
                if (!isExist)
                {
                    return _storageProvider.Add(new AddStorageConfigurationDTO
                    {
                        Type = dto.Type,
                        IsActive = dto.IsActive,
                        Value = dto.Value,
                        Name = dto.Name,
                        ApplicationName = _applicationName
                    });
                }
            }

            return false;
        }

        public async Task<bool> AddAsync(AddConfigurationDTO dto)
        {
            var validationResult = new AddNewConfigurationValidator().Validate(dto);
            if (validationResult.IsValid)
            {
                var isExist = _storageProvider.Exists(dto.Name, _applicationName);
                if (!isExist)
                {
                   return await _storageProvider.AddAsync(new AddStorageConfigurationDTO
                    {
                        Type = dto.Type,
                        IsActive = dto.IsActive,
                        Value = dto.Value,
                        Name = dto.Name,
                        ApplicationName = _applicationName
                    });
                }
            }

            return false;
        }

        public bool Update(UpdateConfigurationDTO dto)
        {
            var validationResult = new UpdateConfigurationValidator().Validate(dto);
            if (validationResult.IsValid)
            {
                return _storageProvider.Update(dto);
            }

            return false;
        }

        public List<ConfigurationDTO> GetAll()
        {
            return _storageProvider.GetList(_applicationName);
        }

        public List<ConfigurationDTO> SearchByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return _storageProvider.Search(name, _applicationName);
            }
            return new List<ConfigurationDTO>();
        }

        public ConfigurationDTO GetById(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                return _storageProvider.Get(id);
            }

            return null;
        }

        private void InitiliazeRefreshCacheTimer()
        {
            FillCacheConfigurationList();

            _timer = new System.Timers.Timer
            {
                Interval = _refreshTimerIntervalInMs,
                AutoReset = true,
                Enabled = true
            };

            _timer.Elapsed += RefreshCache;
            _timer.Start();
        }

        private void FillCacheConfigurationList()
        {
            var list = _storageProvider.GetList(_applicationName);
            var cacheList = list.Select(a => new CacheConfigurationDTO
            {
                ApplicationName = a.ApplicationName,
                Value = a.Value,
                Name = a.Name,
                Type = a.Type,
                Id = a.Id,
                CreationDate = a.CreationDate,
                LastModifyDate = a.LastModifyDate,
                IsActive = a.IsActive
            }).ToList();

            _cacheManager.Add(CacheKey.ApplicationConfigurationParameterList, cacheList);
        }

        private void RefreshCache(object sender, System.Timers.ElapsedEventArgs e)
        {
            var newCachedList = CachedList != null ? new List<CacheConfigurationDTO>(CachedList) : new List<CacheConfigurationDTO>();

            var lastModifyDate = newCachedList.OrderByDescending(a => a.LastModifyDate).FirstOrDefault()?.LastModifyDate;
            var configurationList = _storageProvider.GetList(_applicationName, lastModifyDate ?? DateTime.MinValue);

            if (!configurationList.Any())
                return;

            foreach (var configuration in configurationList)
            {
                var existingCacheItem = newCachedList.FirstOrDefault(a => a.Id == configuration.Id);
                if (existingCacheItem != null)
                {
                    existingCacheItem.Type = configuration.Type;
                    existingCacheItem.Value = configuration.Value;
                    existingCacheItem.LastModifyDate = configuration.LastModifyDate;
                    existingCacheItem.IsActive = configuration.IsActive;
                }
                else
                {
                    newCachedList.Add(new CacheConfigurationDTO
                    {
                        Id = configuration.Id,
                        Type = configuration.Type,
                        LastModifyDate = configuration.LastModifyDate,
                        Value = configuration.Value,
                        Name = configuration.Name,
                        ApplicationName = configuration.ApplicationName,
                        CreationDate = configuration.CreationDate,
                        IsActive = configuration.IsActive
                    });
                }
            }

            _cacheManager.Remove(CacheKey.ApplicationConfigurationParameterList);
            _cacheManager.Add(CacheKey.ApplicationConfigurationParameterList, newCachedList);
        }

        #region Disposable

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _timer.Dispose();
                }
            }

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

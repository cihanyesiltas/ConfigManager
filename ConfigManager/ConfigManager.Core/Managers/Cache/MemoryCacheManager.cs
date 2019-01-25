using System;
using ConfigManager.Core.Contracts;
using Microsoft.Extensions.Caching.Memory;
using ConfigManager.Core.Extensions;

namespace ConfigManager.Core.Managers
{
    public class MemoryCacheManager : ICacheManager, IDisposable
    {
        private readonly MemoryCache _memoryCache;

        public MemoryCacheManager()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public T Get<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out object res))
            {
                return res.ToString().Cast<T>();
            }

            return default(T);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void Add<T>(string key, T value)
        {
            _memoryCache.GetOrCreate(key, entry => value);
        }

        #region Disposable

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _memoryCache.Dispose();
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
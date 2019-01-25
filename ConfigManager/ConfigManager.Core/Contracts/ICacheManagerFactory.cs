using ConfigManager.Core.Enums;

namespace ConfigManager.Core.Contracts
{
    public interface ICacheManagerFactory
    {
        ICacheManager Create(CacheManagerType type);
    }
}

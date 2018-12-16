namespace ConfigManager.Core.Contracts
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        void Remove(string key);
        void Add<T>(string key, T value);
    }
}

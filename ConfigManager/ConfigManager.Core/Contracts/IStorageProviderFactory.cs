using ConfigManager.Core.DTOs;

namespace ConfigManager.Core.Contracts
{
    public interface IStorageProviderFactory
    {
        IStorageProvider Create(Connection connection);
    }
}

using ConfigManager.Core.DTOs;

namespace ConfigManager.Core.Contracts
{
    public interface IConfigurationReaderFactory
    {
        IConfigurationReader Create(string applicationName, Connection connection, int refreshTimerIntervalInMs);
    }
}

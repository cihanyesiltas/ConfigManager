using ConfigManager.Core.Enums;

namespace ConfigManager.Core.DTOs
{
    public class Connection
    {
        public Connection(string  connectionString, StorageProviderType storageProviderType)
        {
            ConnectionString = connectionString;
            StorageProviderType = storageProviderType;
        }

        public string ConnectionString { get; set; }
        public StorageProviderType StorageProviderType { get; set; }
    }
}

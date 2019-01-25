using Microsoft.EntityFrameworkCore;

namespace ConfigManager.Core.Providers.InMemoryDbProvider
{
    internal class ConfigurationContext : DbContext
    {
        public ConfigurationContext()
        {
        }

        public ConfigurationContext(DbContextOptions<ConfigurationContext> options)
            : base(options)
        {
        }
        
        public DbSet<Configuration> Configurations { get; set; }
    }
}
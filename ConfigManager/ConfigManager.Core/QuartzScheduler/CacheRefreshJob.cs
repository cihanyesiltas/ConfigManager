using System.Threading.Tasks;
using ConfigManager.Core.Contracts;
using Quartz;

namespace ConfigManager.Core.QuartzScheduler
{
    public class CacheRefreshJob : IJob
    {
        private readonly IConfigurationReader _configurationReader;
        
        public CacheRefreshJob(IConfigurationReader configurationReader)
        {
            _configurationReader = configurationReader;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _configurationReader.RefreshCache();
            await Task.CompletedTask;
        }
    }
}
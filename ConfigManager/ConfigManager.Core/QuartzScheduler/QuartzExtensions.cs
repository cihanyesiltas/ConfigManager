using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace ConfigManager.Core.QuartzScheduler
{
    public static class QuartzExtensions  
    {  
        public static void AddCacheRefreshQuartzScheduler(this IServiceCollection services, int refreshTime)  
        {  
            services.Add(new ServiceDescriptor(typeof(IJob), typeof(CacheRefreshJob), ServiceLifetime.Transient));  
            services.AddSingleton<IJobFactory, ScheduledJobFactory>();  
            services.AddSingleton<IJobDetail>(provider => JobBuilder.Create<CacheRefreshJob>()  
                .WithIdentity("CacheRefresh.Job", "ScheduleJob")  
                .Build());  
  
            services.AddSingleton<ITrigger>(provider =>  
            {  
                return TriggerBuilder.Create()  
                    .WithIdentity($"CacheRefresh.Trigger", "ScheduleTrigger")  
                    .StartNow()  
                    .WithSimpleSchedule  
                    (s =>  
                        s.WithInterval(TimeSpan.FromMilliseconds(refreshTime))  
                            .RepeatForever()  
                    )  
                    .Build();  
            });  
  
            services.AddSingleton<IScheduler>(provider =>  
            {  
                var schedulerFactory = new StdSchedulerFactory();  
                var scheduler = schedulerFactory.GetScheduler().Result;  
                scheduler.JobFactory = provider.GetService<IJobFactory>();  
                scheduler.Start();  
                return scheduler;  
            });  
        }  
  
        public static void UseQuartz (this IApplicationBuilder app)  
        {  
            app.ApplicationServices.GetService<IScheduler>()  
                .ScheduleJob(app.ApplicationServices.GetService<IJobDetail>(),   
                    app.ApplicationServices.GetService<ITrigger>()  
                );  
        }  
    }  
}
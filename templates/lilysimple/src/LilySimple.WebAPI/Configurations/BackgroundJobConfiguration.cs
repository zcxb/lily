using Hangfire;
using Hangfire.MemoryStorage;
using LilySimple.Services.Rbac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Configurations
{
    public static class BackgroundJobConfiguration
    {
        public static IServiceCollection AddHangfireBackgroundJobs(this IServiceCollection services)
        {
            services.AddHangfireServer();
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage();
            });

            return services;
        }

        public static IApplicationBuilder UseHangfireBackgroundJobs(this IApplicationBuilder app, IBackgroundJobClient backgroundJobClient)
        {
            backgroundJobClient.Enqueue(() =>
            app.ApplicationServices.GetService<RbacService>().InitializeAdminUser());

            return app;
        }
    }
}

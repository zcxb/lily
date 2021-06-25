using Autofac;
using LilySimple.Contexts;
using LilySimple.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutofacModule = Autofac.Module;

namespace LilySimple.Configurations
{
    public static class DbContextConfiguration
    {
        public static IServiceCollection AddDefaultDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkMySql()
                .AddDbContextPool<DefaultDbContext>(options =>
                {
                    options.UseMySql(configuration.GetConnectionString("Default"),
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(typeof(DefaultDbContext).Assembly.FullName);
                            //sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                        });
                });

            return services;
        }

        public static IApplicationBuilder InitDatabase(this IApplicationBuilder app, IConfiguration configuration)
        {
            var optionBuilder = new DbContextOptionsBuilder<DefaultDbContext>();
            optionBuilder.UseMySql(configuration.GetConnectionString("Default"));
            using var dbContext = new DefaultDbContext(optionBuilder.Options);
            dbContext.Database.EnsureCreated();

            return app;
        }
    }

    public class DbContextModule : AutofacModule
    {
        protected override void Load(global::Autofac.ContainerBuilder builder)
        {
            builder.RegisterType<DefaultDbContext>().AsSelf().PropertiesAutowired()
                .InstancePerLifetimeScope();
        }
    }
}

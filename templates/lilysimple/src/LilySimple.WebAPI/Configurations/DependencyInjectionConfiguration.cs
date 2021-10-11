using Autofac;
using Autofac.Extensions.DependencyInjection;
using LilySimple.EventBus;
using LilySimple.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System.Linq;
using AutofacModule = Autofac.Module;

namespace LilySimple.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IApplicationBuilder UseIocManager(this IApplicationBuilder app)
        {
            Autofac.IocManager.Instance.ServiceProvider = app.ApplicationServices.GetAutofacRoot();

            return app;
        }
    }

    public class ApplicationModule : AutofacModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            var exportedTypes = typeof(ServiceBase).Assembly.GetExportedTypes().ToList();

            builder.RegisterTypes(exportedTypes.Where(t => typeof(ServiceBase).IsAssignableFrom(t)).ToArray())
                .InstancePerLifetimeScope().AsSelf().PropertiesAutowired();
            builder.RegisterTypes(exportedTypes.Where(t => typeof(IEventHandler).IsAssignableFrom(t)).ToArray())
                .InstancePerLifetimeScope().AsImplementedInterfaces().PropertiesAutowired();
        }
    }
}

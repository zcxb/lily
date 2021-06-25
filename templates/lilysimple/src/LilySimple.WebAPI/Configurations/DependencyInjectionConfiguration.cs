using Autofac;
using Autofac.Extensions.DependencyInjection;
using LilySimple.Autofac;
using LilySimple.Services;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
            var assemblies = ReflectionHelper.GetAllAssemblies();

            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(t => typeof(ServiceBase).IsAssignableFrom(t) && !t.IsAbstract)
                .AsSelf()
                .InstancePerLifetimeScope(); 
        }
    }

    public class DefaultDbContextModule : AutofacModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Contexts.DefaultDbContext>().AsSelf().PropertiesAutowired()
                .InstancePerLifetimeScope();
        }
    }
}

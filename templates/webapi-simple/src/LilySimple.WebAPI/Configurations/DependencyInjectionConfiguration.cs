using Autofac.Extensions.DependencyInjection;
using LilySimple.Autofac;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Configurations
{
    public static class DependencyInjectionConfiguration
    {
        public static IApplicationBuilder UseIocManager(this IApplicationBuilder app)
        {
            IocManager.Instance.ServiceProvider = app.ApplicationServices.GetAutofacRoot();

            return app;
        }
    }
}

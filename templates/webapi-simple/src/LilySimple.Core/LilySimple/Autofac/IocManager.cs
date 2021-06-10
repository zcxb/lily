using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Autofac
{
    public class IocManager
    {
        public static IocManager Instance { get; set; }

        public ILifetimeScope ServiceProvider { get; set; }

        static IocManager()
        {
            Instance = new IocManager();
        }

        public TService GetService<TService>()
        {
            return ServiceProvider.Resolve<TService>();
        }
    }
}

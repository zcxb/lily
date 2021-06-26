using LilySimple.Autofac;
using LilySimple.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Services
{
    public class ServiceBase
    {
        protected readonly IConfiguration Configuration;

        public Contexts.DefaultDbContext Db { get; set; }
        public ServiceBase()
        {
            Configuration = IocManager.Instance.GetService<IConfiguration>();
        }
    }
}

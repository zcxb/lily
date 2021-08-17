using LilySimple.Autofac;
using LilySimple.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Services
{
    public class ServiceBase
    {
        protected readonly IConfiguration Configuration;

        public ILogger<ServiceBase> Logger { protected get; set; }
        public Contexts.DefaultDbContext Db { protected get; set; }
        public ServiceBase()
        {
            Configuration = IocManager.Instance.GetService<IConfiguration>();
        }
    }
}

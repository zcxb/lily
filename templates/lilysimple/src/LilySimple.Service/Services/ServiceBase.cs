using LilySimple.Autofac;
using LilySimple.Contexts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.Services
{
    public class ServiceBase
    {
        protected readonly DefaultDbContext Db;
        protected readonly IConfiguration Configuration;

        public ServiceBase()
        {
            Db = IocManager.Instance.GetService<DefaultDbContext>();
            Configuration = IocManager.Instance.GetService<IConfiguration>();
        }
    }
}

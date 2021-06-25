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
        //protected readonly DefaultDbContext Db;
        protected readonly IConfiguration Configuration;

        //public ServiceBase(DefaultDbContext db)
        public ServiceBase()
        {
            //Db = db;
            Configuration = IocManager.Instance.GetService<IConfiguration>();
        }
    }
}

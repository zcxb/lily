using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Hangfire;
using LilySimple.Configurations;
using LilySimple.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LilySimple
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceBase).Assembly);
            services.AddCustomPermission();
            services.AddMiniProfiler().AddEntityFramework();
            services.AddDefaultDbContext(Configuration);
            services.AddCustomJwtBearerAuthentication(Configuration);
            services.AddCustomSwagger();
            services.AddHangfireBackgroundJobs();
            services.AddControllers().AddCustomJsonOptions();
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IBackgroundJobClient backgroundJobClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIocManager();
            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiniProfiler();
            app.UseCustomMiddlewares();
            app.UseCustomEndpoints(Configuration);
            app.InitDatabase(Configuration);
            app.UseHangfireBackgroundJobs(backgroundJobClient);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DbContextModule());
            builder.RegisterModule(new ApplicationModule());
        }
    }
}

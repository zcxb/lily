using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Configurations
{
    public static class EndpointsConfiguration
    {
        public static IApplicationBuilder UseCustomEndpoints(this IApplicationBuilder app, IConfiguration configuration)
        {
            var useSwagger = bool.TryParse(configuration["Swagger:Enabled"], out var swaggerEnabled) && swaggerEnabled;
            if (useSwagger)
            {
                app.UseCustomSwagger();
            }
            app.UseEndpoints(endpoints =>
            {
                if (useSwagger)
                {
                    endpoints.MapGet("/", context =>
                    {
                        context.Response.Redirect("swagger");
                        return Task.CompletedTask;
                    });
                }
                endpoints.MapControllers();
            });

            return app;
        }
    }
}

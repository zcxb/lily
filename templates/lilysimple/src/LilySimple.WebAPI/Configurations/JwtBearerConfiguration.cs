using LilySimple.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LilySimple.Configurations
{
    public static class JwtBearerConfiguration
    {
        private static AuthenticationBuilder AddCustomAuthentication(this IServiceCollection services)
        {
            return services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
        }

        private static AuthenticationBuilder AddCustomJwtBearer(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            builder.AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtBearer:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtBearer:SecurityKey"])),
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            return builder;
        }

        public static AuthenticationBuilder AddCustomJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .Configure<JwtBearerSetting>(configuration.GetSection("JwtBearer"))
                .AddCustomAuthentication()
                .AddCustomJwtBearer(configuration);
        }
    }
}

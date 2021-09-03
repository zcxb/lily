using LilySimple.DataStructure.Tree;
using LilySimple.JsonConverters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LilySimple.Configurations
{
    public static class JsonOptionsConfiguration
    {
        public static IMvcBuilder AddCustomJsonOptions(this IMvcBuilder services)
        {
            services.AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new TreeNodeConverter());
                options.JsonSerializerOptions.Converters.Add(new UnixTimestampConverter());
            });

            return services;
        }
    }
}

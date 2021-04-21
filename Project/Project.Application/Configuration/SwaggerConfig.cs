using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace Project.Application.Configuration
{
    public static class SwaggerConfig
    {
        /// <summary>
        /// Startup.cs: ConfigureServices
        /// </summary>
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "1.0",
                    Title = "Amandasdn.Project.Api",
                    Description = "Source: https://github.com/amandasdn/ASPNET_Core_REST_Dapper"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                x.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            return services;
        }

        /// <summary>
        /// Startup.cs: Configure
        /// </summary>
        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = string.Empty;
                x.SwaggerEndpoint("/swagger/v1.0/swagger.json", "v1.0");
            });

            return app;
        }
    }
}

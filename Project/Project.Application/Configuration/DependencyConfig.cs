using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Project.Domain.Interfaces;
using Project.Infra.Repositories;
using Project.Service.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Project.Application.Configuration.SwaggerConfig;

namespace Project.Application.Configuration
{
    public static class DependencyConfig
    {
        /// <summary>
        /// Startup.cs: ConfigureServices
        /// </summary>
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.TryAddScoped<ICategoryRepository, CategoryRepository>();
            services.TryAddScoped<ICategoryService, CategoryService>();

            services.TryAddScoped<IProductRepository, ProductRepository>();
            services.TryAddScoped<IProductService, ProductService>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}

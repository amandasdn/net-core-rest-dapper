using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Project.Domain.Interfaces;
using Project.Infra.Repositories;
using Project.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            return services;
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Project.Domain.Interfaces;
using Project.Infra.Repositories;
using Project.Service.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Project.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "1.0",
                    Title = "Amandasdn.Project.Api",
                    Description = "https://github.com/amandasdn/ASPNET_Core_REST_Dapper"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                x.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            services.TryAddScoped<IProductRepository, ProductRepository>();
            services.TryAddScoped<IProductService, ProductService>();

            services.TryAddScoped<ICategoryRepository, CategoryRepository>();
            services.TryAddScoped<ICategoryService, CategoryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(x =>
            {
                x.RoutePrefix = string.Empty;
                x.SwaggerEndpoint("/swagger/v1.0/swagger.json", "v1.0");
            });
        }
    }
}

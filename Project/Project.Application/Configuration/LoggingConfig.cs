using KissLog;
using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Text;

namespace Project.Application.Configuration
{
    public static class LoggingConfig
    {
        /// <summary>
        /// Startup.cs: ConfigureServices
        /// </summary>
        public static IServiceCollection AddKissLog(this IServiceCollection services)
        {
            services.AddScoped<ILogger>((context) =>
            {
                return Logger.Factory.Get();
            });

            services.AddLogging(logging =>
            {
                logging.AddKissLog();
            });

            return services;
        }

        /// <summary>
        /// Startup.cs: Configure
        /// </summary>
        public static IApplicationBuilder UseKissLog(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseKissLogMiddleware(options => {
                ConfigureKissLog(options, configuration);
            });

            return app;
        }

        private static void ConfigureKissLog(IOptionsBuilder options, IConfiguration configuration)
        {
            // optional KissLog configuration
            options.Options
                .AppendExceptionDetails((Exception ex) =>
                {
                    StringBuilder sb = new StringBuilder();

                    if (ex is System.NullReferenceException nullRefException)
                    {
                        sb.AppendLine("Important: check for null references");
                    }

                    return sb.ToString();
                });

            // KissLog internal logs
            options.InternalLog = (message) =>
            {
                Debug.WriteLine(message);
            };

            // register logs output
            RegisterKissLogListeners(options, configuration);
        }

        private static void RegisterKissLogListeners(IOptionsBuilder options, IConfiguration configuration)
        {
            // multiple listeners can be registered using options.Listeners.Add() method

            // register KissLog.net cloud listener
            options.Listeners.Add(new RequestLogsApiListener(new KissLog.CloudListeners.Auth.Application(
                configuration["KissLog.OrganizationId"],    //  "9758c9e1-d674-43e3-94dd-b185b7b318f8"
                configuration["KissLog.ApplicationId"])     //  "7bd2a6a8-7cde-482a-8dff-bb4e7f645d3e"
            )
            {
                ApiUrl = configuration["KissLog.ApiUrl"]    //  "https://api.kisslog.net"
            });
        }
    }
}

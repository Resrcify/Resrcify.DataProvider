using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Quartz;
using Serilog;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Infrastructure.BackgroundJobs;
using Titan.DataProvider.Infrastructure.Caching;
using Titan.DataProvider.Infrastructure.HttpClients;

namespace Titan.DataProvider.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            var apiUrl = Environment.GetEnvironmentVariable("API");

            if (bool.TryParse(Environment.GetEnvironmentVariable("ISTITAN"), out var isTitan) && isTitan)
                services.AddHttpClient<IGalaxyOfHeroesService, GalaxyOfHeroesService>(c =>
                {
                    c.BaseAddress = new Uri(apiUrl ?? "http://localhost:10000");
                }).AddPolicyHandler(GetRetryPolicy());
            else
                services.AddHttpClient<IGalaxyOfHeroesService, ComlinkService>(c =>
                {
                    c.BaseAddress = new Uri(apiUrl ?? "http://localhost:3200");
                })
                .AddPolicyHandler(GetRetryPolicy());

            services.AddDistributedMemoryCache();
            services.AddSingleton<ICachingService, CachingService>();


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(CheckMetadataVersionJob));
                configure
                    .AddJob<CheckMetadataVersionJob>(jobKey)
                    .AddTrigger(
                        trigger =>
                            trigger.ForJob(jobKey)
                                .StartAt(DateTime.UtcNow.AddSeconds(30))
                                .WithSimpleSchedule(
                                    schedule =>
                                        schedule.WithIntervalInMinutes(15)
                                            .RepeatForever()));

                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService();
            return services;
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
                .OrResult(msg => msg.StatusCode == HttpStatusCode.Unauthorized)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
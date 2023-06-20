using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Quartz;
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
            var clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL") ?? "http://localhost";
            var port = Environment.GetEnvironmentVariable("PORT") ?? "3200";
            if (bool.TryParse(Environment.GetEnvironmentVariable("IS_TITAN"), out var isTitan) && isTitan)
                services.AddHttpClient<IGalaxyOfHeroesService, GalaxyOfHeroesService>(c =>
                {
                    if (port == "3200") port = "10000";
                    var uri = clientUrl + ":" + port;
                    c.BaseAddress = new Uri(uri);
                })
                .AddPolicyHandler(GetRetryPolicy())
                .ConfigurePrimaryHttpMessageHandler(messageHandler =>
                {
                    var handler = new HttpClientHandler();
                    if (handler.SupportsAutomaticDecompression)
                        handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    return handler;
                });
            else
                services.AddHttpClient<IGalaxyOfHeroesService, ComlinkService>(c =>
                {
                    var uri = clientUrl + ":" + port;
                    c.BaseAddress = new Uri(uri);
                })
                .AddPolicyHandler(GetRetryPolicy())
                .ConfigurePrimaryHttpMessageHandler(messageHandler =>
                {
                    var handler = new HttpClientHandler();
                    if (handler.SupportsAutomaticDecompression)
                        handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    return handler;
                });

            services.AddDistributedMemoryCache();
            services.AddSingleton<ICachingService, CachingService>();

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
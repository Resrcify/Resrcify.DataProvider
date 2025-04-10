using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Quartz;
using Resrcify.DataProvider.Application.Abstractions;
using Resrcify.DataProvider.Infrastructure.BackgroundJobs;
using Resrcify.DataProvider.Infrastructure.HttpClients;
using Resrcify.SharedKernel.Caching.Abstractions;
using Resrcify.SharedKernel.Caching.Primitives;

namespace Resrcify.DataProvider.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL") ?? configuration.GetValue<string>("ClientUrl");
        var port = Environment.GetEnvironmentVariable("PORT") ?? configuration.GetValue<string>("ClientPort");

        services.AddHttpClient<IGalaxyOfHeroesService, GalaxyOfHeroesService>(c =>
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
        services.AddSingleton<ICachingService, InMemoryCachingService>();

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
        });

        services.AddQuartzHostedService();
        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
            .OrResult(msg => msg.StatusCode == HttpStatusCode.Unauthorized)
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
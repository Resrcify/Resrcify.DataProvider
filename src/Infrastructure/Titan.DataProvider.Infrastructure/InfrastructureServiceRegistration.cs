using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.DataProvider.Infrastructure.Caching;
using Titan.DataProvider.Infrastructure.HttpClients;

namespace Titan.ShardManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            var apiUrl = Environment.GetEnvironmentVariable("API");
            services.AddHttpClient<IGalaxyOfHeroesWrapperService, GalaxyOfHeroesWrapperService>(c =>
            {
                c.BaseAddress = new Uri(apiUrl ?? "http://localhost:10000");
            }).AddPolicyHandler(GetRetryPolicy());

            services.AddHttpClient<IComlinkService, ComlinkService>(c =>
            {
                c.BaseAddress = new Uri(apiUrl ?? "http://localhost:5000");
            })
            .AddPolicyHandler(GetRetryPolicy());

            services.AddDistributedMemoryCache();
            services.AddSingleton<ICachingService, CachingService>();


            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();
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
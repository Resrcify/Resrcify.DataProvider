using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Titan.DataProvider.Application.Abstractions.Infrastructure;
using Titan.ShardManagement.Infrastructure.GalaxyOfHeroesWrapper;

namespace Titan.ShardManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            var apiUrl = Environment.GetEnvironmentVariable("API");
            services.AddHttpClient<IComlinkService, ComlinkService>(c =>
            {
                c.BaseAddress = new Uri(apiUrl ?? "http://localhost:10000");
            })
            .AddPolicyHandler(GetRetryPolicy());
            services.AddDistributedMemoryCache();
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
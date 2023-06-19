using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Titan.DataProvider.API;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
            .UseSerilog((context, configuration) =>
                configuration
                .WriteTo.Console()
                .MinimumLevel.Information()
            )
            .Build()
            .Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://*:18000");
                webBuilder.UseStartup<Startup>();
            });
}

using System.Runtime.InteropServices;
using MaoXianCat.Cli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
#if !DEBUG
    Directory.SetCurrentDirectory(AppContext.BaseDirectory);
#endif

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args)
        .ConfigureLogging(builder =>
        {
            builder.AddConsole();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                builder.AddEventLog(settings =>
                {
                    settings.LogName = "Application";
                    settings.SourceName = "FilePorter";
                });   
            }
        })
        .ConfigureAppConfiguration(builder =>
        {
            builder
#if !DEBUG
                .SetBasePath(Directory.GetCurrentDirectory())
#endif
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json", true);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<MonitorWork>();
            services.Configure<FilePorterOptions>(hostContext.Configuration.GetSection("FilePorter"));
        });

    hostBuilder = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
        ? hostBuilder.UseWindowsService(options =>
        {
            options.ServiceName = ".NET FilePorter Service";
        })
        : hostBuilder.UseSystemd();
    return hostBuilder;
}
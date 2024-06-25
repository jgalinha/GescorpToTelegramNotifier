using GescorpToTelegramNotifier.Host.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true);
        config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
        config.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
        config.AddEnvironmentVariables();
        config.AddCommandLine(args);
        
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddServices(hostContext.Configuration);
        services.Configure<GescorpApiOptions>(hostContext.Configuration.GetSection("GescorpApi"));
        services.AddLogging();
        services.AddMemoryCache();
    })
    .Build();

var gescorpApiClient = host.Services.GetRequiredService<GescorpApiClient>();
var incident = await gescorpApiClient.GetIncidentsAsync();
Console.ReadKey();

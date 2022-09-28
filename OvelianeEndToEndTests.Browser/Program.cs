var hostBuilder = Host.CreateDefaultBuilder(args);
var configurationRoot = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

var applicationConfiguration = new ApplicationConfiguration();
configurationRoot.Bind(applicationConfiguration);

hostBuilder.UseSerilog((_, config) => config.ReadFrom.Configuration(configurationRoot));

var host = hostBuilder
    .ConfigureServices((_, services) =>
    {
        services
            .AddOptions()
            .AddSingleton<EndToEndTestsApplication>()
            .AddSingleton<IWebDriverFactory, FirefoxDriverFactory>()
            .AddSingleton(applicationConfiguration);
    })
    .UseConsoleLifetime()
    .Build();

using var serviceScope = host.Services.CreateScope();
var services = serviceScope.ServiceProvider;
var application = services.GetRequiredService<EndToEndTestsApplication>();
application.Run();
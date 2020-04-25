using System.IO;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace SketchFabApi.Samples
{
    class Program
    {
        static async Task Main(string[] args)
        {

            // Load config
            // Load appsettings.json
            var config = BuildConfiguration();
            if (null == config)
            {
                Console.WriteLine("Missing or invalid appsettings.json file.");
                return;
            }

            // Setting up dependency injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(config, serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            await serviceProvider.GetRequiredService<SketchFabSample>().Run();

        }

        private static IConfigurationRoot BuildConfiguration()
        {
            try
            {

                var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("secrets.json", optional: true, reloadOnChange: false)
                .Build();


                return config;
            }
            catch (System.IO.FileNotFoundException)
            {
                return null;
            }
        }


        private static void ConfigureServices(IConfigurationRoot config, IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                config.AddDebug(); // Log to debug (debug window in Visual Studio or any debugger attached)
                config.AddConsole(o =>
                {
                    o.IncludeScopes = false;
                    o.DisableColors = false;
                }); // Log to console (colored !)
            })
           .Configure<LoggerFilterOptions>(options =>
           {
               options.AddFilter<DebugLoggerProvider>(null /* category*/ , LogLevel.Information /* min level */);
               options.AddFilter<ConsoleLoggerProvider>(null  /* category*/ , LogLevel.Information /* min level */);

               // Comment this line to see all internal DEM.Net logs
               //options.AddFilter<ConsoleLoggerProvider>("DEM.Net", LogLevel.Information);
           });

            services.AddOptions();
            services.Configure<AppSecrets>(config.GetSection(nameof(AppSecrets)));


            services.AddScoped<SketchFab.SketchFabApi>();
            services.AddSingleton<SketchFabSample>();


        }
    }
}

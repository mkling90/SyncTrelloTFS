using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SyncTrelloTFS.Business;
using SyncTrelloTFS.Services;

namespace SyncTrelloTFS
{
    class Program
    {
        static void Main(string[] args)
        {

            //Create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            //entry to run app
            serviceProvider.GetService<App>().Run(args);
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            //Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));
            //Add services
            serviceCollection.AddTransient<IAppService, AppService>();
            serviceCollection.AddTransient<ITrelloApi, TrelloApi>();
            serviceCollection.AddTransient<ITFSApi, TFSApi>();
            serviceCollection.AddTransient<ITrelloToTFS, TrelloToTFS>();
            serviceCollection.AddTransient<ITrelloReportService, TrelloReportService>();

            //add app
            serviceCollection.AddTransient<App>();
        }
    }
}

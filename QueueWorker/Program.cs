using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMqBindings;

namespace QueueWorker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureWebJobs(jobs =>
                {
                    jobs.AddCosmosDB();
                    jobs.AddRabbitMq();
                })
                .ConfigureAppConfiguration((host, app) =>
                {
                    app.SetBasePath(Directory.GetCurrentDirectory());
                    app.AddJsonFile("appsettings.json", true);
                    app.AddEnvironmentVariables();
                })
                .ConfigureLogging((host, logging) =>
                {
                    logging.AddConsole();

                    string appInsightsKey = host.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                    if (!string.IsNullOrEmpty(appInsightsKey))
                    {
                        // This uses the options callback to explicitly set the instrumentation key.
                        logging.AddApplicationInsights(o =>
                        {
                            o.InstrumentationKey = appInsightsKey;
                        });
                    }
                })
                .ConfigureServices((host, services) =>
                {

                })
                .UseConsoleLifetime()
                .Build();

            await builder.RunAsync();
        }
    }
}

using DesktopWeatherReport.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Windows.Forms;

namespace DesktopWeatherReport
{
    internal static class Program
    {

        public static IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {

            Log.Logger = new LoggerConfiguration()

            .WriteTo.SQLite(@".\..\..\..\Database\log.db")
            .CreateLogger();

            try
            {
                Log.Information("Starting up");
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var host = CreateHostBuilder().Build();
                ServiceProvider = host.Services;

                Application.Run(ServiceProvider.GetRequiredService<DesktopWeatherReportForm>());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureServices((context, services) =>
                {
                    services.AddHttpClient();
                    services.AddSingleton<IOpenWeatherMapController, OpenWeatherMapController>();
                    services.AddSingleton<IImageConfigurationController, ImageConfigurationController>();
                    services.AddTransient<DesktopWeatherReportForm>();
                });
            // may need to dispose of host appropriately after program exit:
            // https://stackoverflow.com/questions/59301011/ihostedservice-stopasync-vs-dispose
        }
    }
}
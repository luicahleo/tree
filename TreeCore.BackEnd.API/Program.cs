using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using TreeCore.APIClient;
using TreeCore.BackEnd.API.Settings;
using TreeCore.BackEnd.WorkerServices;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.API
{
    public class Program
    {
        public static string Environment = "";
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    Environment = env.EnvironmentName;
                    //env.EnvironmentName = "Release";
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("SharedSetting.json", optional: true, reloadOnChange: true);
                     Environment = env.EnvironmentName;

                    if (env.EnvironmentName == "Local")
                    {
                        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                        if (appAssembly != null)
                        {
                            config.AddUserSecrets(appAssembly, optional: true);
                        }
                    }

                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }

                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<DbConnection>(x => new SqlConnection(DatabaseConnection.BuildConnectionString(hostContext.Configuration, Environment)));
                    services.AddSingleton<TransactionalWrapper>();
                    services.AddHostedService<WorkerService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })                
                .UseNLog();
    }
}

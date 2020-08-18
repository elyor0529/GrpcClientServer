using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GrpcServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Grpc Server";

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Limits.MaxRequestBodySize = null; 
                    });

                    webBuilder.UseSerilog((hst, cnf) =>
                    {
                        cnf.MinimumLevel.Error();
                        cnf.ReadFrom.Configuration(hst.Configuration);
                        cnf.Enrich.FromLogContext();
                        cnf.Enrich.WithProperty("ApplicationName", hst.HostingEnvironment.ApplicationName);
                        cnf.WriteTo.ColoredConsole();
                        cnf.WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true);
                    });

                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GrpcServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    { 
                        options.Limits.MinRequestBodyDataRate = null;
                        options.ListenAnyIP(5001, o =>
                        {
                            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                            var certPath = Path.Combine(basePath!, "Certs", "server1.pfx");

                            o.UseHttps(certPath, "1111");
                            o.Protocols = HttpProtocols.Http2;
                        });

                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}

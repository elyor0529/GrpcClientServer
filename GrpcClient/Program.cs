using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Grpc Client";

            var tasks = new Task[1000];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = BatchProcess(i);
            };
            Task.WaitAll(tasks);

            Console.WriteLine("Done!");
        }

        private static async Task BatchProcess(int clientNumber)
        {
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            using var channel = GrpcChannel.ForAddress("https://178.33.123.109:5000", new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });
            var client = new Greeter.GreeterClient(channel);

            for (var i = 0; i < 10; i++)
            {
                var file = Path.Combine(Environment.CurrentDirectory, "users.json");
<<<<<<< HEAD
                var startTime = DateTime.Now;
=======
                var startTime = DateTime.Now; // Environment.TickCount64;
>>>>>>> 4dda7046d456ae9b50b75fd1adfbe21fa330ab31
                var data = await File.ReadAllTextAsync(file);
                var response = await client.SayHelloAsync(new HelloRequest
                {
                    Name = $"Client{clientNumber} pushing {Path.GetFileName(file)}",
                    Data = data
                });
<<<<<<< HEAD
                var endTime = DateTime.Now;
                var timer = endTime.Subtract(startTime);
=======
                var endTime = DateTime.Now;                
                var timer = (endTime - startTime).Duration();
>>>>>>> 4dda7046d456ae9b50b75fd1adfbe21fa330ab31

                Console.WriteLine($"Pulled from server:{response.Message}({timer:g})");
            }
        }

    }
}
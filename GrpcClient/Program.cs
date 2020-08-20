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

            Parallel.For(0, 100, async (i) =>
            {
                BatchProcess(i).Wait();
            });

            Console.WriteLine("Done!");
        }

        private static async Task BatchProcess(int part)
        {
            Console.WriteLine("Channel #" + part);

            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            var channel = GrpcChannel.ForAddress("https://178.33.123.109:5001", new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });
            var client = new Greeter.GreeterClient(channel);

            for (var i = 0; i < 1000; i++)
            {
                var file = Path.Combine(Environment.CurrentDirectory, "users.json");

                await PostFile(client, i, file);
            }
        }

        private static async Task PostFile(Greeter.GreeterClient client, int part, string file)
        {
            var timer = new Stopwatch();
            timer.Start();
            var data = await File.ReadAllTextAsync(file);
            var response = await client.SayHelloAsync(new HelloRequest
            {
                Name = $"Client{part} sending {Path.GetFileName(file)}",
                Data = data
            });
            timer.Stop();

            Console.WriteLine("Server pull:{0}({1:g})", response.Message, timer.Elapsed);
        }
    }
}
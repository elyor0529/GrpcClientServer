using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.Title = "Grpc Client";

            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            var channel = GrpcChannel.ForAddress("https://178.33.123.109:5001", new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });
            var client = new Greeter.GreeterClient(channel);
            Console.WriteLine("Client ping");

            for (var i = 0; i < 100; i++)
            {
                var timer = new Stopwatch();
                timer.Start();
                var file = i % 2 == 0 ? "orders.json" : "customers.json";
                var data = await File.ReadAllTextAsync(file);
                var response = await client.SayHelloAsync(new HelloRequest
                {
                    Name = $"client1 sending {file}",
                    Data = data
                });
                timer.Stop();
                Console.WriteLine("Server pull:{0}({1:g})", response.Message, timer.Elapsed);
            }

            Console.WriteLine("Done!");
        }
    }
}
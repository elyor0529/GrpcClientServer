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

            BatchProcess();

            Console.WriteLine("Done!");
        }

        private static void BatchProcess()
        {

            var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "db"), "*.json", SearchOption.AllDirectories);

            Enumerable.Range(0, 5000)
                 .AsParallel()
                 .ForAll(async (part) =>
                 {
                     var random = new Random();
                     var order = random.Next(0, files.Length);
                     var file = files[order];

                     await PostFile(part, file);
                 });
        }

        private static async Task PostFile(int part, string file)
        {
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            var channel = GrpcChannel.ForAddress("https://178.33.123.109:5001", new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });
            var client = new Greeter.GreeterClient(channel);
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
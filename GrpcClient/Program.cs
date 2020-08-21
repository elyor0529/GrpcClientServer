﻿using System;
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
                tasks[i] = Task.Run(async () => await BatchProcess(i));
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
            using var channel = GrpcChannel.ForAddress("https://178.33.123.109:5001", new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });
            var client = new Greeter.GreeterClient(channel);

            for (var i = 0; i < 100; i++)
            {
                var file = Path.Combine(Environment.CurrentDirectory, "users.json");
                var startTime = Environment.TickCount64;
                var data = await File.ReadAllTextAsync(file);
                var response = await client.SayHelloAsync(new HelloRequest
                {
                    Name = $"Client{clientNumber} pushing {Path.GetFileName(file)}",
                    Data = data
                });
                var endTime = Environment.TickCount64;
                var timer = new TimeSpan(endTime - startTime);

                Console.WriteLine($"Pulled from server:{response.Message}({timer:g})");
            }
        }

    }
}
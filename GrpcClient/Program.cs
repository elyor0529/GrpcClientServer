using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    internal static class Program
    {
        private static async Task Main(string[] args)
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
            Console.WriteLine("Client ping");

            var response = await client.SayHelloAsync(new HelloRequest
            {
                Name = "client1"
            });
            Console.WriteLine("Server pull:{0}", response.Message);

            Console.WriteLine("Done!{0}",args);
        }
    }
}
#nullable enable
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;

namespace GrpcClient
{
    internal static class Program
    {

        private static byte[]? GetBytesFromPem(string pemString, string section)
        {
            var header = $"-----BEGIN {section}-----";
            var footer = $"-----END {section}-----";
            var start = pemString.IndexOf(header, StringComparison.Ordinal);

            if (start == -1)
            {
                return null;
            }

            start += header.Length;
            var end = pemString.IndexOf(footer, start, StringComparison.Ordinal) - start;

            return end == -1 ? null : Convert.FromBase64String(pemString.Substring(start, end));
        }

        private static async Task<GrpcChannel> CreateChannel()
        {

            var httpClientHandler = new HttpClientHandler();
            var pem = await File.ReadAllTextAsync("Certs/client1.pem");
            var certData = GetBytesFromPem(pem, "CERTIFICATE");
            var cert = new X509Certificate2(certData);

            httpClientHandler.ClientCertificates.Add(cert);

            var channel = GrpcChannel.ForAddress($"https://localhost:5001", new GrpcChannelOptions
            {
                Credentials = new SslCredentials(),
                HttpHandler = httpClientHandler,
            });

            return channel;
        }

        private static async Task Main(string[] args)
        {
            var channel = await CreateChannel();
            
            var client = new Greeter.GreeterClient(channel);
            Console.WriteLine("Client ping");
            
            var response = await client.SayHelloAsync(new HelloRequest
            {
                Name = "client1"
            });
            Console.WriteLine("Server pull:{0}", response.Message);

            Console.WriteLine("Done!");
        }
    }
}

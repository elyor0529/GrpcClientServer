using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcServer.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var reply = new HelloReply
            {
                Message = "Tuk : " + DateTime.Now.ToString("F") + " , File: " + request.Name + " , Bytes:" + request.Data.Length
            };

            Task.Run(() =>
            {
                // Notify logging
            });

            var userId = "00000000-0000-0000-0000-000000000000";
            if (!string.IsNullOrWhiteSpace(userId))
            {
                // Checking account 
            }

            Task.Run(() =>
            {
                // Pushing queue
            });

            return Task.FromResult(reply);
        }
    }
}
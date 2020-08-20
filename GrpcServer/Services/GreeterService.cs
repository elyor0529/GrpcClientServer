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
            Task.Run(() =>
            {
                _logger.LogInformation("Request:{0}", request.Name);
            });

            return Task.FromResult(new HelloReply
            {
                Message = "Tuk : " + DateTime.Now.ToString("F") + " , File: " + request.Name + " , Bytes:" + request.Data.Length
            });
        }
    }
}
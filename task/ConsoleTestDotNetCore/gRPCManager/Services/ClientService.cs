using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Grpc.Core;
using gRPCClient;
using Microsoft.Extensions.Logging;

namespace gRPCClient
{
    public class ClientClient : Client.ClientBase
    {
        private readonly ILogger<ClientClient> _logger;

        public ClientClient(ILogger<ClientClient> logger)
        {
            _logger = logger;
        }

        public override Task<test> Test(test request, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information,$"Manager receive the {request}");
            return Task.FromResult(new test
            {
                Data = $"Manager receive the {request}"
            });
        }

        public override Task<test> TestCells(Cells request, ServerCallContext context)
        {
            return Task.FromResult(new test()
            {
                Data = "Success"
            });
        }
    }
}
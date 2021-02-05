using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using gRPCClient;
using gRPCStructures;
using gRPCWorker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Status = gRPCStructures.Status;

namespace gRPCManager.Services
{
    public class ClientsService : Client.ClientBase
    {
        private readonly ILogger<ClientsService> _logger;
        private Worker.WorkerClient worker;
        private GrpcChannel channel;
        public ClientsService(ILogger<ClientsService> logger)
        {
            // пока добавляю только одного воркера для теста, потом увеличу их кол-во
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", isEnabled: true);
            channel = GrpcChannel.ForAddress(
                "http://localhost:5002", new GrpcChannelOptions()
                {
                    Credentials = ChannelCredentials.Insecure,
                    LoggerFactory = new NullLoggerFactory()
                });
            worker = new Worker.WorkerClient(channel);
            _logger = logger;
        }

        public override Task<test> Test(test request, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information,$"Manager receive the {request}");
            return Task.FromResult(new test
            {
                Data = worker.Test(new test{Data = request.Data}).Data
            });
        }
        
        public override Task<Cells> Generate(Size request, ServerCallContext context)
        {
            return Task.FromResult(worker.Generate(request));
        }

        public override Task<Cells> NextGeneration(Cells request, ServerCallContext context)
        {
            return Task.FromResult(worker.NextGeneration(request));
        }

        public override Task<Status> ChangeStatus(Status request, ServerCallContext context)
        {
            return Task.FromResult(worker.ChangeStatus(request));
        }
    }
}
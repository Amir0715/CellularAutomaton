using System.Threading.Tasks;
using Grpc.Core;
using gRPCClient;
using gRPCStructures;
using gRPCWorker;
using Microsoft.Extensions.Logging;
using Status = gRPCStructures.Status;

namespace gRPCManager.Services
{
    public class ClientsService : Client.ClientBase
    {
        private readonly ILogger<ClientsService> _logger;
        private Worker.WorkerClient worker;
        private Manager manager;
        public ClientsService(ILogger<ClientsService> logger)
        {
            _logger = logger;
            manager = new Manager();
            manager.AddWorkerClient(manager.CreateNewWorker("http://localhost:5002"));
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
            return Task.FromResult(manager.Generate(request));
        }

        public override Task<Cells> NextGeneration(Cells request, ServerCallContext context)
        {
            return Task.FromResult(manager.NextGeneration(request));
        }

        public override Task<Status> ChangeStatus(Status request, ServerCallContext context)
        {
            return Task.FromResult(manager.WorkerClients[0].ChangeStatus(request));
        }
        
    }
}
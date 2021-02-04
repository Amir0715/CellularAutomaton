using System;
using Automaton.core;
using Grpc.Core;
using Grpc.Net.Client;
using gRPCClient;
using Microsoft.Extensions.Logging.Abstractions;
using Cell = Automaton.core.Cell;

namespace Avalonia.NETCoreApp
{
    // TODO: ВЫПИЛИТЬ КЛАСС AUTOMATON
    public class Frontend
    {
        private AutomatonBase Automaton;
        private static Frontend _instance;
        private Client.ClientClient client; // клиент который будет связан с manager
        private GrpcChannel Channel; // канал для связи 
        public Cells Data;

        private Frontend(int cols, int rows)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", isEnabled: true);
            Channel = GrpcChannel.ForAddress(
                "http://localhost:5001", new GrpcChannelOptions()
                {
                    Credentials = ChannelCredentials.Insecure,
                    LoggerFactory = new NullLoggerFactory()
                });
            client = new Client.ClientClient(Channel);

            Automaton = new AutomatonBase(cols, rows); 
            Data = new Cells();
            Generate();
        }

        public static Frontend GetInstance(int cols = 0, int rows = 0)
        {
            return _instance ??= new Frontend(cols, rows);
        }

        public void Start()
        {
            Automaton.Start();
        }

        public void Stop()
        {
            Automaton.Stop();
        }

        public void NextGeneration()
        {
            Data.Update(Automaton.NextGeneration());
            client.NextGeneration(Data);
        }

        public void NextStep()
        {
            Start();
            NextGeneration();
            Stop();
        }

        public void Generate()
        {
            Data.Update(Automaton.Generate());
            client.Generate(Data);
        }
        
        public void SetCell(int x, int y) // can be optimize
        {
            Automaton.SetCell(x, y);
            Data.SetCell(x, y);
        }
    }
}
using System;
using System.Collections.Generic;
using Automaton.core;
using Grpc.Core;
using Grpc.Net.Client;
using gRPCClient;
using gRPCStructures;
using Microsoft.Extensions.Logging.Abstractions;
using Cells = Automaton.core.Cells;
using Status = gRPCStructures.Status;


namespace Avalonia.NETCoreApp
{
    public class Frontend
    {
        // private AutomatonBase Automaton;
        private static Frontend _instance;
        private Client.ClientClient client; // клиент который будет связан с manager
        private GrpcChannel Channel; // канал для связи 
        public Cells Data;
        private int rows;
        private int cols;
        private bool IsStarted;

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
            var call = client.Generate(
                new gRPCStructures.Size
                {
                    Cols = cols,
                    Rows = rows
                }
            );
            var recv = call;
            var forU = GCellsToCells(recv);
            Data = new Cells();
            Data.Update(forU);

            this.cols = cols;
            this.rows = rows;
        }

        public static Frontend GetInstance(int cols = 0, int rows = 0)
        {
            return _instance ??= new Frontend(cols, rows);
        }

        public void Start()
        {
            client.ChangeStatus(new Status
            {
                Data = true
            });
        }

        public void Stop()
        {
            client.ChangeStatus(new Status
            {
                Data = false
            });
        }

        public void NextGeneration()
        {
            if (IsStarted)
            {
                var call = client.NextGeneration(CellsToGCells(Data));
                var recv = call;
                Data = GCellsToCells(recv);
            }
        }

        public void NextStep()
        {
            IsStarted = true;
            NextGeneration();
            IsStarted = false;
            
        }

        public void Generate()
        {
            // Data.Update(Automaton.Generate());
            var call = client.Generate(
                new gRPCStructures.Size
                {
                    Cols = cols,
                    Rows = rows
                }
            );
            var recv = call;
            Data.Update(GCellsToCells(recv));
        }
        
        public void SetCell(int x, int y) 
        {
            if (!IsStarted)
                Data.SetCell(x, y);
        }
        
        // иначе ругается на разные типы, т.к. Cells определен в Automaton
        public static gRPCStructures.Cells CellsToGCells(Cells param)
        {
            var rows = new List<gRPCStructures.Cells.Types.RowCell>();
            for (var i = 0; i < param.Data.Length; i++)
            {
                var row = new List<gRPCStructures.Cells.Types.RowCell.Types.Cell>();
                for (var j = 0; j < param[0].Length; j++)
                {
                    var cell = new gRPCStructures.Cells.Types.RowCell.Types.Cell
                    {
                        IsAlive = param[i][j].IsAlive,
                        NumberOfNeighbors = param[i][j].NumberOfNeighbors,
                        Value = param[i][j].Value
                    };
                    row.Add(cell);
                }

                var rowCell = new gRPCStructures.Cells.Types.RowCell() {Data = {row}};
                rows.Add(rowCell);
            }

            var res = new gRPCStructures.Cells() {Data = {rows}};
            return res;
        }

        public static Cells GCellsToCells(gRPCStructures.Cells c)
        {
            int i = 0, j = 0;
            var data = new Cell[c.Data.Count][];
            foreach (var x in c.Data)
            {
                data[i] = new Cell[x.Data.Count];
                foreach (var y in x.Data)
                {
                    data[i][j++] = new Cell
                    {
                        IsAlive = y.IsAlive,
                        NumberOfNeighbors = y.NumberOfNeighbors,
                        Value = y.Value
                    };
                }

                i++;
                j = 0;
            }
            return new Cells(data);
        }
        
    }
}
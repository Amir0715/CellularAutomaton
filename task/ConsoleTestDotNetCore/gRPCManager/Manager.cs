using System;
using System.Collections.Generic;
using Automaton.core;
using Grpc.Core;
using Grpc.Net.Client;
using gRPCStructures;
using gRPCWorker;
using Microsoft.Extensions.Logging.Abstractions;

using Cells = Automaton.core.Cells;
namespace gRPCManager
{
    public class Manager
    {
        public List<Worker.WorkerClient> WorkerClients;

        public Manager()
        {
            WorkerClients = new List<Worker.WorkerClient>();
        }

        public Worker.WorkerClient CreateNewWorker(string url)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", 
                isEnabled: true);
            var channel = GrpcChannel.ForAddress(
                url, new GrpcChannelOptions()
                {
                    Credentials = ChannelCredentials.Insecure,
                    LoggerFactory = new NullLoggerFactory()
                });
            return new Worker.WorkerClient(channel);
        }

        public void AddWorkerClient(Worker.WorkerClient workerClient)
        {
            WorkerClients.Add(workerClient);
        }

        public gRPCStructures.Cells Generate(Size request)
        {
            var result = new Cells();
            var step = request.Rows / WorkerClients.Count;
            var size = new Size {Cols = request.Cols, Rows = step};
            for (var i = 0; i < WorkerClients.Count-1; i++)
            {
                result += GCellsToCells(WorkerClients[i].Generate(size));
            }
            if (WorkerClients.Count == 1)
            {
                result = GCellsToCells(WorkerClients[^1].Generate(size));
            }
            else
            {
                // TODO : CHECK
                size.Rows = request.Rows % WorkerClients.Count;
                result += GCellsToCells(WorkerClients[^1].Generate(size));
            }
            return CellsToGCells(result);
        }

        public gRPCStructures.Cells NextGeneration(gRPCStructures.Cells request)
        {
            var cells = GCellsToCells(request);
            var step = cells.Data.GetLength(0) / WorkerClients.Count;
            var result = new Cells();
            var start = 0;
            var end = step;
            for (var i = 0; i < WorkerClients.Count - 1; i++)
            {
                var gcell = CellsToGCells(cells.GetFromTo(start, end));
                result += GCellsToCells(WorkerClients[i].NextGeneration(gcell));
                start += step;
                end += step;
            }
            if (WorkerClients.Count == 1)
            {
                var gcell = CellsToGCells(cells.GetFromTo(start, end));
                result = GCellsToCells(WorkerClients[^1].NextGeneration(gcell));
            }
            else
            {
                var gcell = CellsToGCells(cells.GetFromTo(start, cells.Data.GetLength(0)));
                result += GCellsToCells(WorkerClients[^1].NextGeneration(gcell));
            }

            return CellsToGCells(result);
        }
        
        
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
            Cell[][] data = new Cell[c.Data.Count][];
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
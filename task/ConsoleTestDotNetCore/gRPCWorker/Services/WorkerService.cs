using System.Collections.Generic;
using System.Threading.Tasks;
using Automaton.core;
using Grpc.Core;
using gRPCStructures;
using Microsoft.Extensions.Logging;
using Cells = Automaton.core.Cells;
using Status = gRPCStructures.Status;

// Класс воркера -- объект который должен проводить вычесления 
namespace gRPCWorker.Services
{
    public class WorkerService : Worker.WorkerBase
    {
        private readonly ILogger<WorkerService> _logger;
        private AutomatonBase AutomatonObj;
        private object locker = new object();
        private int cols;
        private int rows;
        
        public WorkerService(ILogger<WorkerService> logger)
        {
            _logger = logger;
        }

        public override Task<test> Test(test request, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information,$"Worker receive the {request}");
            return Task.FromResult(new test{Data = "From worker"});
        }
        
        public override Task<gRPCStructures.Cells> Generate(Size request, ServerCallContext context)
        {
            this.cols = request.Cols;
            this.rows = request.Rows;
            Cells res;
            lock (locker)
            {
                AutomatonObj = AutomatonBase.GetInstance(cols, rows);
                res = AutomatonObj.Generate();
            }

            return Task.FromResult(CellsToGCells(res));
        }

        public override Task<Status> ChangeStatus(Status request, ServerCallContext context)
        {
            bool res;
            lock (locker)
            {
                AutomatonObj = AutomatonBase.GetInstance();
                res = AutomatonObj.ChangeStatus();
            }
            return Task.FromResult(new Status{Data = res});
        }

        public override Task<gRPCStructures.Cells> NextGeneration(gRPCStructures.Cells request, ServerCallContext context)
        {
            Cells res;
            lock (locker)
            {
                AutomatonObj = AutomatonBase.GetInstance();
                res = GCellsToCells(request);
                res = AutomatonObj.NextGeneration(res);
            }
            return Task.FromResult(CellsToGCells(res));
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
                j = 0;
                i++;
            }

            return new Cells(data);
        }
    }
}
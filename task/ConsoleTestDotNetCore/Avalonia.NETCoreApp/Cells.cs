using System.Collections.Generic;
using Automaton.core;

namespace Avalonia.NETCoreApp
{
    public class Cells
    {
        public Cell[][] data { get; private set; }
    
        public int Length()
        {
            return data.Length; 
        }
        
        public Cells(Cell[][] data)
        {
            this.data = data;
        }

        public Cells()
        {
        }

        public void Update(Cell[][] data)
        {
            this.data = data;
        }
        
        public Cell this[int i, int j]
        {
            get
            {
                return data[i][j];
            }
            set
            {
                data[i][j] = value;
            }
        }

        public Cell SetCell(int x, int y, Cell c)
        {
            this.data[x][y] = c;
            return data[x][y];
        }

        public Cell SetCell(int x, int y)
        {
            Cell c = data[x][y];
            this.data[x][y] = new Cell {IsAlive = !c.IsAlive, Value = c.Value, NumberOfNeighbors = c.NumberOfNeighbors};
            return this.data[x][y];
        }
        

        public Cell[] this[int i]
        {
            get
            {
                return data[i];
            }
        }

        public static implicit operator gRPCClient.Cells(Cells param)
        {
            var rows = new List<gRPCClient.Cells.Types.RowCell>();
            for (var i = 0; i < param.data.Length; i++)
            {
                var row = new List<gRPCClient.Cells.Types.RowCell.Types.Cell>();
                for (var j = 0; j < param[0].Length; j++)
                {
                    var cell = new gRPCClient.Cells.Types.RowCell.Types.Cell
                    {
                        IsAlive = param[i][j].IsAlive,
                        NumberOfNeighbors = param[i][j].NumberOfNeighbors,
                        Value = param[i][j].Value
                    };
                    row.Add(cell);
                }

                var RowCell = new gRPCClient.Cells.Types.RowCell() {Data = {row}};
                rows.Add(RowCell);
            }

            var res = new gRPCClient.Cells() {Data = {rows}};
            return res;
        }
    }
}
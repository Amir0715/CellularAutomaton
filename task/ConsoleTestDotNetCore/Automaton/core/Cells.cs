using System.Collections.Generic;

namespace Automaton.core
{
    public class Cells
    {
        public Cell[][] Data { get; private set; }

        public int Length()
        {
            return Data.Length;
        }

        public Cells(Cell[][] data)
        {
            this.Data = data;
        }

        public Cells()
        {
        }

        public void Update(Cell[][] data)
        {
            this.Data = data;
        }

        public void Update(Cells c)
        {
            this.Data = c.Data;
        }

        public Cell this[int i, int j]
        {
            get { return Data[i][j]; }
            set { Data[i][j] = value; }
        }

        public Cell SetCell(int x, int y, Cell c)
        {
            this.Data[x][y] = c;
            return Data[x][y];
        }

        public Cell SetCell(int x, int y)
        {
            Cell c = Data[x][y];
            this.Data[x][y] = new Cell {IsAlive = !c.IsAlive, Value = c.Value, NumberOfNeighbors = c.NumberOfNeighbors};
            return this.Data[x][y];
        }


        public Cell[] this[int i]
        {
            get { return Data[i]; }
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
                data[i++] = new Cell[x.Data.Count];
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
            }

            return new Cells(data);
        }
        
    }
}
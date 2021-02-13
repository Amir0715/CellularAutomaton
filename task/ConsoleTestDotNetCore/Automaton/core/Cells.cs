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

        public Cells(int cols, int rows)
        {
            Data = new Cell[cols][];
            for (var i = 0; i < cols; i++)
            {
                Data[i] = new Cell[rows];
                for (var j = 0; j < rows; j++)
                {
                    Data[i][j] = new Cell();
                }
            }
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
            var c = Data[x][y];
            this.Data[x][y] = new Cell {IsAlive = !c.IsAlive, Value = c.Value, NumberOfNeighbors = c.NumberOfNeighbors};
            return this.Data[x][y];
        }


        public Cell[] this[int i]
        {
            get { return Data[i]; }
        }

        public Cells GetFromTo(int irow, int jrow) // TODO irow < data[0].length [irow, jrow)
        {
            var result = new Cells(this.Data.Length, jrow-irow + 2);
            if (irow != 0)
            {
                for (var i = 0; i < this.Data.Length; i++)
                {
                    result.Data[i][0] = this.Data[i][irow-1];
                }
            }
            if (jrow != this.Data[0].Length)
            {
                for (var i = 0; i < this.Data.Length; i++)
                {
                    result.Data[i][^1] = this.Data[i][jrow];
                }
            }
            for (var i = 0; i < this.Data.Length; i++)
            {
                var k = 1;
                for (var j = irow; j < jrow; j++)
                {
                    result.Data[i][k++] = this.Data[i][j];
                }
            }

            return result;
        }
        
        public static Cells operator +(Cells c1, Cells c2) // TODO fix c2.Data
        {
            if (c1.Data == null)
            {
                return c2;
            }
            
            var cols1 = c1.Data.Length;
            var rows1 = c1.Data[0].Length; // TODO fix c1.Data.GetLength(1)
            
            var cols2 = c2.Data.Length;
            var rows2 = c2.Data[0].Length; // TODO fix c2.Data.GetLength(1)
            Cells result;
            if (cols1 == cols2)
            {
                result = new Cells(cols1, rows1+rows2);
                for (var i = 0; i < cols1; i++)
                {
                    for (var j = 0; j < rows1; j++)
                    {
                        result.Data[i][j] = c1.Data[i][j];
                    }
                    // TODO CHECK
                    for (var j = rows1; j < rows1+rows2; j++)
                    {
                        result.Data[i][j] = c2.Data[i][j-rows1];
                    }
                }
            }else if (cols1 != cols2 && rows1 == rows2)
            {
                result = new Cells(cols1+cols2, rows1);
                for (var i = 0; i < cols1; i++)
                {
                    for (var j = 0; j < rows1; j++)
                    {
                        result[i][j] = c1[i][j];
                    }
                }
                for (var i = cols1; i < cols2; i++)
                {
                    for (var j = 0; j < rows1; j++)
                    {
                        result[i][j] = c1[i-cols1][j];
                    }
                }
            }
            else
            {
                result = new Cells();
            }
            return result;
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
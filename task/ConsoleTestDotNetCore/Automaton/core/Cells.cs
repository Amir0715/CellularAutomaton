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

        public Cells GetFromTo(int irow, int jrow)
        {
            var result = new Cells(rows:jrow-irow, cols:this.Data.Length);
            for (var i = 0; i < jrow - irow; i++)
            {
                result.Data[i] = this.Data[i];
            }

            return result;
        }
        
        public static Cells operator +(Cells c1, Cells c2)
        {
            var cols1 = c1.Data.GetLength(0);
            var rows1 = c1.Data.GetLength(1);
            
            var cols2 = c2.Data.GetLength(0);
            var rows2 = c2.Data.GetLength(1);
            Cells result;
            if (cols1 == cols2)
            {
                result = new Cells(cols1, rows1+rows2);
                for (var j = 0; j < cols1; j++)
                {
                    for (var i = 0; i < rows1; i++)
                    {
                        result[i][j] = c1[i][j];
                    }

                    for (var i = rows1; i < rows2; i++)
                    {
                        result[i][j] = c2[i - rows1][j];
                    }
                }
                
            }else if (rows1 == rows2)
            {
                result = new Cells(cols1+cols2, rows1);
                for (var i = 0; i < rows1; i++)
                {
                    for (var j = 0; j < cols1; j++)
                    {
                        result[i][j] = c1[i][j];
                    }
                    for (var j = cols1; j < cols2; j++)
                    {
                        result[i][j] = c2[i][j-cols1];
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
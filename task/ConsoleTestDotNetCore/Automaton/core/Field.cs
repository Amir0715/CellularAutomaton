using System;
using System.Threading.Tasks;

namespace Automaton.core
{
    public class Field
    {
        public Cell[][] Data { get; set; }
        private Func<Cell, Cell> Transform { set; get; }
        private TaskManager TaskManager;
        private int CountOfCores { get; set; }
        private int Columns { get; set; }
        private int Rows { get; set; }

        public Field(int cols, int rows, Func<Cell, Cell> transform) : this(cols, rows)
        {
            Transform = transform;
        }

        public Field(int cols, int rows)
        {
            if (cols < 0)
            {
                throw new RowsOrColumnsLessZeroException("Columns can't be less 0");
            }
            if (rows < 0)
            {
                throw new RowsOrColumnsLessZeroException("Rows can't be less 0");
            }
            Rows = rows;
            Columns = cols;
            Data = init(cols, rows);
            CountOfCores = 4;
        }

        private Cell[][] init(int cols, int rows)
        {
            var result = new Cell[cols][];
            for (var i = 0; i < cols; i++)
            {
                result[i] = new Cell[rows];
                for (var j = 0; j < rows; j++)
                {
                    result[i][j] = new Cell();
                }
            }

            return result;
        }
        
        public Cell[][] Generate(int cols, int rows)
        {
            var r = new Random();
            var res = init(cols, rows);
            for (var i = 0; i < cols; i++)
            {
                for (var j = 0; j < rows; j++)
                {
                    res[i][j].Generate(r);
                }
            }
            return res;
        }
        
        public Cell[][] NextGeneration()
        {
            // TODO CHECK
            Columns = Data.Length;
            Rows = Data[0].Length;
            
            var tmp = new Cell[Columns][];
            for (var i = 0; i < Columns; i++)
            {
                tmp[i] = new Cell[Rows];
            }

            TaskManager = new TaskManager();
            var lengthOfOneRange = Columns / CountOfCores;
            
            for (var i = 0; i < CountOfCores; i++) {
                var indexStart = i * lengthOfOneRange;
                var indexEnd = (i + 1) * lengthOfOneRange;
                TaskManager.AddTask(i == CountOfCores - 1
                    ? new Task(() => UpdateNumbersOfNeighborsTask(indexStart, Columns))
                    : new Task(() => UpdateNumbersOfNeighborsTask(indexStart, indexEnd)));
            }
            
            TaskManager.RunAll();
            TaskManager.WaitAll();
            TaskManager.Clear();
            
            for (var i = 0; i < CountOfCores; i++) {
                var indexStart = i * lengthOfOneRange;
                var indexEnd = (i + 1) * lengthOfOneRange;
                TaskManager.AddTask(i == CountOfCores - 1
                    ? new Task(() => NextGenerationCell(ref tmp, indexStart, Columns))
                    : new Task(() => NextGenerationCell(ref tmp, indexStart, indexEnd)));
            }
            
            TaskManager.RunAll();
            TaskManager.WaitAll();
            TaskManager.Clear();
            
            var tmp2 = new Cell[Columns][];
            for (var i = 0; i < Columns; i++)
            {
                tmp2[i] = new Cell[Rows-2];
                var k = 0;
                for (int j = 1; j <= Rows-2; j++)
                {
                    tmp2[i][k++] = tmp[i][j];
                }
            }
            Data = tmp2;
            return Data;
        }
        
        private void NextGenerationCell(ref Cell[][] tmp, int indexStart, int indexEnd)
        {
            for (var i = indexStart; i < indexEnd; i++)
            {
                for (var j = 1; j < Rows-1; j++)
                {
                    tmp[i][j] = Data[i][j].Life();
                }
            }
        }

        private void UpdateNumbersOfNeighborsTask(int indexStart, int indexEnd)
        {
            for (var i = indexStart; i < indexEnd; i++)
            {
                for (var j = 1; j < Rows-1; j++)
                {
                    UpdateNumbersOfNeighbors(i, j);
                }
            }
        }
        
        private void UpdateNumbersOfNeighbors(int x , int y)
        {
            var countOfNeighbors = 0;
            
            for(var i = -1; i <= 1; i++)
            {
                for (var j = -1 ; j <= 1; j++)
                {
                    var col = x + i;
                    var row = y + j;
                    if (col < 0 || col >= Columns)
                        continue;

                    if( ! ( (col == x ) && (row == y) ) && Data[col][row].IsAlive)
                    {
                        countOfNeighbors++;
                    }
                }
            }
            Data[x][y].NumberOfNeighbors = countOfNeighbors;
        }

        public void SetCell(int x, int y)
        {
            Data[x][y].IsAlive = !Data[x][y].IsAlive;
        }

        public Cell[][] Clear()
        {
            Data = new Cell[this.Columns][];
            for (var i = 0; i < this.Columns; i++)
            {
                Data[i] = new Cell[this.Rows];
                for (var j = 0; j < this.Rows; j++)
                {
                    Data[i][j] = new Cell();
                }
            }

            return Data;
        }
    }
}

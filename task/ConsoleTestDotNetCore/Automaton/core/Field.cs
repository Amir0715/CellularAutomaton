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
            Data = new Cell[cols][];
            for (var i = 0; i < cols; i++)
            {
                Data[i] = new Cell[rows];
                for (var j = 0; j < rows; j++)
                {
                    Data[i][j] = new Cell();
                }
            }
            CountOfCores = 4;
        }

        public Cell[][] Generate()
        {
            var r = new Random();
            for (var i = 0; i < Columns; i++)
            {
                for (var j = 0; j < Rows; j++)
                {
                    Data[i][j].Generate(r);
                }
            }
            return Data;
        }
        
        public Cell[][] NextGeneration()
        {
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
            Data = tmp;
            return Data;
        }
        
        private void NextGenerationCell(ref Cell[][] tmp, int indexStart, int indexEnd)
        {
            for (var i = indexStart; i < indexEnd; i++)
            {
                for (var j = 0; j < Rows; j++)
                {
                    
                    tmp[i][j] = Data[i][j].Life();
                }
            }
        }

        private void UpdateNumbersOfNeighborsTask(int indexStart, int indexEnd)
        {
            for (var i = indexStart; i < indexEnd; i++)
            {
                for (var j = 0; j < Rows; j++)
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
                    var col = (x + i + Columns) % Columns;
                    var row = (y + j + Rows) % Rows;
                   
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

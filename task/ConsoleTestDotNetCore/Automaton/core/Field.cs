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
            this.Transform = transform;
        }

        public Field(int cols, int rows)
        {
            if (cols < 0)
            {
                throw new RowsOrColumnsLessZeroException("Columns can't be less 0");
            }else if (rows < 0)
            {
                throw new RowsOrColumnsLessZeroException("Rows can't be less 0");
            }
            this.Rows = rows;
            this.Columns = cols;
            Data = new Cell[cols][];
            for (var i = 0; i < cols; i++)
            {
                Data[i] = new Cell[rows];
                for (var j = 0; j < rows; j++)
                {
                    Data[i][j] = new Cell();
                }
            }
            CountOfCores = 3;
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
                    ? new Task(() => UpdateNumbersOfNeigborsTask(indexStart, Columns))
                    : new Task(() => UpdateNumbersOfNeigborsTask(indexStart, indexEnd)));
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
            //NextGenerationCell(ref tmp, 0, 5);
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

        private void UpdateNumbersOfNeigborsTask(int indexStart, int indexEnd)
        {
            for (var i = indexStart; i < indexEnd; i++)
            {
                for (var j = 0; j < Rows; j++)
                {
                    UpdateNumbersOfNeigbors(i, j);
                    
                }
            }
        }
        
        private void UpdateNumbersOfNeigbors(int x , int y)
        {
            var countOfNeigbors = 0;
            
            for(var i = -1; i <= 1; i++)
            {
                for (var j = -1 ; j <= 1; j++)
                {
                    var col = (x + i + Columns) % Columns;
                    var row = (y + j + Rows) % Rows;
                   
                    if( ! ( (col == x ) && (row == y) ) && Data[col][row].IsAlive)
                    {
                        countOfNeigbors++;
                    }
                }
            }
            Data[x][y].NumberOfNeigbors = countOfNeigbors;
        }

        public void SetCell(int x, int y, Cell c)
        {
            Data[x][y].IsAlive = !Data[x][y].IsAlive;
        }
    }
}

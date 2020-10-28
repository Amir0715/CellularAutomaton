using System;
using System.Threading.Tasks;

namespace Automaton.core
{
    public class Field
    {
        public Cell[][] Data { get; private set; }
        public Func<Cell, Cell> transform { set; get; }
        private TaskManager taskManager;
        private int countOfCores;
        private int columns;
        private int rows;

        public Field(int columns, int rows, Func<Cell, Cell> transform)
        {
            this.rows = rows;
            this.columns = columns;
            this.transform = transform;
            Data = new Cell[columns][];
            for (int i = 0; i < columns; i++)
            {
                Data[i] = new Cell[rows];
                for (int j = 0; j < rows; j++)
                {
                    Data[i][j] = new Cell();
                }
            }
            countOfCores = 4;
        }

        public Field(int columns, int rows)
        {
            this.rows = rows;
            this.columns = columns;
            Data = new Cell[columns][];
            for (int i = 0; i < columns; i++)
            {
                Data[i] = new Cell[rows];
                for (int j = 0; j < rows; j++)
                {
                    Data[i][j] = new Cell();
                }
            }
            countOfCores = 4;
        }

        public void Generate()
        {
            Random r = new Random();
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Data[i][j].Generate(r);
                }
            }
        }
        
        public void NextGeneration()
        {
            Cell[][] tmp = new Cell[columns][];
            for (int i = 0; i < columns; i++)
            {
                tmp[i] = new Cell[rows];
            }

            taskManager = new TaskManager();
            int lengthOfOneRange = columns / countOfCores;

            for (int i = 0; i < countOfCores; i++) {
                int index_start = i * lengthOfOneRange;
                int index_end = (i + 1) * lengthOfOneRange;
                if (i == countOfCores - 1)
                    taskManager.AddTask(new Task(() => NextGenerationCell(ref tmp, index_start, columns)));
                else
                    taskManager.AddTask(new Task(() => NextGenerationCell(ref tmp, index_start, index_end)));
            }

            taskManager.RunAll();
            taskManager.WaitAll();
            
            Data = tmp;
        }


        private void NextGenerationCell(ref Cell[][] tmp, int indexStart, int indexEnd)
        {
            for (int i = indexStart; i < indexEnd; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    UpdateNumbersOfNeigbors(i, j);
                    tmp[i][j] = transform(Data[i][j]);
                }
            }
        }
        
        private void UpdateNumbersOfNeigbors(int x , int y)
        {
            int countOfNeigbors = 0;
            for(int i = - 1; i <= 1; i++)
            {
                for (int j =  - 1 ; j <= 1; j++)
                {
                    var col = (x + i + columns) % columns;
                    var row = (y + j + rows) % rows;

                    if( ( (col != x ) && (row != y) ) && Data[col][row].IsAlive)
                    {
                        countOfNeigbors++;
                    }
                }
            }
            Data[x][y].NumberOfNeigbors = countOfNeigbors;
        }

    }
}

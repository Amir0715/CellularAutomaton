using System;
using System.Threading.Tasks;
using Cellular_automaton;
using Cellular_automaton.source;

namespace ConsoleTestDotNetCore.core
{
    /// <summary>
    /// Класс поля отвечающий за хранения состояния клеток и их перехода в другие состояния.
    /// </summary>
    class Field
    {
        public Cell[][] data { get; set; }
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
            data = new Cell[columns][];
            for (int i = 0; i < columns; i++)
            {
                data[i] = new Cell[rows];
                for (int j = 0; j < rows; j++)
                {
                    data[i][j] = new Cell();
                }
            }
            countOfCores = 4;
        }

        public Field(int columns, int rows)
        {
            this.rows = rows;
            this.columns = columns;
            data = new Cell[columns][];
            for (int i = 0; i < columns; i++)
            {
                data[i] = new Cell[rows];
                for (int j = 0; j < rows; j++)
                {
                    data[i][j] = new Cell();
                }
            }
            countOfCores = 4;
        }


        /// <summary>
        /// Генерация поля
        /// </summary>
        public void Generate()
        {
            Random r = new Random();
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    data[i][j].Generate(r);
                }
            }
        }

        /// <summary>
        /// Вычесляет след. поколение в многопоточном режиме.
        /// </summary>
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
            
            data = tmp;
        }

        /// <summary>
        /// Вычесляет след. состояния клеткок и записывает это в tmp.
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="indexStart">Начальный индекс массива в двумерном массиве</param>
        /// <param name="indexEnd">Конечный индекс массива в двумерном массиве</param>
        private void NextGenerationCell(ref Cell[][] tmp, int indexStart, int indexEnd)
        {
            for (int i = indexStart; i < indexEnd; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    UpdateNumbersOfNeigbors(i, j);
                    tmp[i][j] = transform(data[i][j]);
                }
            }
        }

        /// <summary>
        /// Пересчитываем количество живых соседей для клетки с индексами indexX и indexY
        /// </summary>
        private void UpdateNumbersOfNeigbors(int x , int y)
        {
            int countOfNeigbors = 0;
            for(int i = - 1; i <= 1; i++)
            {
                for (int j =  - 1 ; j <= 1; j++)
                {
                    var col = (x + i + columns) % columns;
                    var row = (y + j + rows) % rows;

                    if( ( (col != x ) && (row != y) ) && data[col][row].isAlive)
                    {
                        countOfNeigbors++;
                    }
                }
            }
            data[x][y].numberOfNeigbors = countOfNeigbors;
        }

    }
}

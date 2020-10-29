﻿using System;
using System.Threading.Tasks;

namespace Automaton.core
{
    public class Field
    {
        private Cell[][] Data { get; set; }
        private Func<Cell, Cell> Transform { set; get; }
        private TaskManager taskManager;
        private int CountOfCores { get; set; }
        private int Columns { get; set; }
        private int Rows { get; set; }

        public Field(int columns, int rows, Func<Cell, Cell> transform) : this(columns, rows)
        {
            this.Transform = transform;
        }

        public Field(int columns, int rows)
        {
            if (columns < 0)
            {
                throw new RowsOrColumnsLessZeroException("Columns can't be less 0");
            }else if (rows < 0)
            {
                throw new RowsOrColumnsLessZeroException("Rows can't be less 0");
            }
            this.Rows = rows;
            this.Columns = columns;
            Data = new Cell[columns][];
            for (var i = 0; i < columns; i++)
            {
                Data[i] = new Cell[rows];
                for (var j = 0; j < rows; j++)
                {
                    Data[i][j] = new Cell();
                }
            }
            CountOfCores = 4;
        }

        public void Generate()
        {
            var r = new Random();
            for (var i = 0; i < Columns; i++)
            {
                for (var j = 0; j < Rows; j++)
                {
                    Data[i][j].Generate(r);
                }
            }
        }
        
        public void NextGeneration()
        {
            var tmp = new Cell[Columns][];
            for (var i = 0; i < Columns; i++)
            {
                tmp[i] = new Cell[Rows];
            }

            taskManager = new TaskManager();
            var lengthOfOneRange = Columns / CountOfCores;

            for (var i = 0; i < CountOfCores; i++) {
                var indexStart = i * lengthOfOneRange;
                var indexEnd = (i + 1) * lengthOfOneRange;
                taskManager.AddTask(i == CountOfCores - 1
                    ? new Task(() => NextGenerationCell(ref tmp, indexStart, Columns))
                    : new Task(() => NextGenerationCell(ref tmp, indexStart, indexEnd)));
            }

            taskManager.RunAll();
            taskManager.WaitAll();
            
            Data = tmp;
        }


        private void NextGenerationCell(ref Cell[][] tmp, int indexStart, int indexEnd)
        {
            for (var i = indexStart; i < indexEnd; i++)
            {
                for (var j = 0; j < Rows; j++)
                {
                    UpdateNumbersOfNeigbors(i, j);
                    tmp[i][j] = Transform(Data[i][j]);
                }
            }
        }
        
        private void UpdateNumbersOfNeigbors(int x , int y)
        {
            var countOfNeigbors = 0;
            for(var i = - 1; i <= 1; i++)
            {
                for (var j =  - 1 ; j <= 1; j++)
                {
                    var col = (x + i + Columns) % Columns;
                    var row = (y + j + Rows) % Rows;

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

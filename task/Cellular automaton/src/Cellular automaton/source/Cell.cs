using System;

namespace Cellular_automaton
{
    /// <summary>
    /// Класс клетки хранящий состояние клетки
    /// </summary>
    class Cell
    {
        public float value { get; set; }
        public int numberOfNeigbors { get; set; }
        public bool isAlive { get; set; }

        public void Generate(Random r)
        {
            value = r.Next(1000);
            isAlive = r.Next(100) < 50;
        }
    }
}

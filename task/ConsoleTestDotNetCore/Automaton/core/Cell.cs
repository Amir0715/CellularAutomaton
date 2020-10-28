using System;

 namespace Automaton.core
{
    public class Cell
    {
        public float Value { get; set; }
        public int NumberOfNeigbors { get; set; }
        public bool IsAlive { get; set; }

        public void Generate(Random r)
        {
            Value = r.Next(1000);
            IsAlive = r.Next(100) < 50;
        }
    }
}

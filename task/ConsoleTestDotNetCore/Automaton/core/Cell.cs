using System;

 namespace Automaton.core
{
    public class Cell
    {
        public float Value { get; set; }
        public int NumberOfNeighbors { get; set; }
        public bool IsAlive { get; set; }

        public void Generate(Random r)
        {
            Value = r.Next(1000);
            IsAlive = r.Next(100) < 50;
        }
        
        public Cell Increment()
        {
            this.Value += 1;
            this.IsAlive = true;
            return this;
        }

        public Cell Decrement()
        {
            this.Value -= 1;
            this.IsAlive = false;
            return this;
        }

        public Cell Life()
        {
            if (this.IsAlive)
            {
                if (NumberOfNeighbors == 2 || NumberOfNeighbors == 3)
                    IsAlive = true;
                if (NumberOfNeighbors < 2 || NumberOfNeighbors > 3)
                    IsAlive = false;
            }
            else
            {
                if (NumberOfNeighbors == 3)
                    IsAlive = true;
            }
            return this;
        }

        public Cell Nothing()
        {
            return this;
        }
    }
}

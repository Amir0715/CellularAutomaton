using System;

 namespace Automaton.core
{
    public class Cell
    {
        private float Value { get; set; }
        public int NumberOfNeigbors { get; set; }
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
                if (NumberOfNeigbors < 2 || NumberOfNeigbors > 3)
                    IsAlive = false;
            }
            else
            {
                if (NumberOfNeigbors == 3)
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

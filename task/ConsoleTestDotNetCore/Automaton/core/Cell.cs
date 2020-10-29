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
        
        public Cell Incriment(Cell cell)
        {
            cell.Value = cell.Value + 1;
            cell.IsAlive = true;
            return cell;
        }

        public Cell Dicriment(Cell cell)
        {
            cell.Value = cell.Value - 1;
            cell.IsAlive = false;
            return cell;
        }

        public Cell Life(Cell cell)
        {
            if (cell.IsAlive)
            {
                if (cell.NumberOfNeigbors < 2 || cell.NumberOfNeigbors > 3)
                    cell.IsAlive = false;
            }
            else
            {
                if (cell.NumberOfNeigbors == 3)
                    cell.IsAlive = true;
            }
            return cell;
        }

        public Cell Nothing(Cell cell)
        {
            return cell;
        }
    }
}

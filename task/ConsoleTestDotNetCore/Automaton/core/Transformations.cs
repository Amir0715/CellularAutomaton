namespace Automaton.core
{
    /// <summary>
    /// Класс преоброзований типа <Cell> -> <Cell>
    /// </summary>
    class Transformations
    {
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

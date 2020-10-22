using System;

namespace Cellular_automaton
{
    /// <summary>
    /// Класс преоброзований типа <Cell> -> <Cell>
    /// </summary>
    class Transformations
    {
        public Cell Incriment(Cell cell)
        {
            cell.value = cell.value + 1;
            cell.isAlive = true;
            return cell;
        }

        public Cell Dicriment(Cell cell)
        {
            cell.value = cell.value - 1;
            cell.isAlive = false;
            return cell;
        }

        public Cell Life(Cell cell)
        {
            if (cell.isAlive)
            {
                if (cell.numberOfNeigbors < 2 || cell.numberOfNeigbors > 3)
                    cell.isAlive = false;
            }
            else
            {
                if (cell.numberOfNeigbors == 3)
                    cell.isAlive = true;
            }
            return cell;
        }

        public Cell Nothing(Cell cell)
        {
            return cell;
        }
    }
}

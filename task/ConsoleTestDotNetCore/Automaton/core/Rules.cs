﻿namespace Automaton.core
{
    /// <summary>
    /// Класс предикатов для задания правил показа
    /// </summary>
    class Rules
    {
        public bool Rule_1(Cell cell)
        {
            return cell.IsAlive;
        }

    }
}

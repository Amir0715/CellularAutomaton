using System;

namespace Automaton.core
{
    public class TaskIsNullException : Exception
    {
        public TaskIsNullException(string message) : base(message)
        { }
    }

    public class RowsOrColumnsLessZeroException : Exception
    {
        public RowsOrColumnsLessZeroException(string message) : base(message)
        {
        }
    }

    public class TasksSizeIsNullException : Exception
    {
        public TasksSizeIsNullException(string massage) : base(massage)
        {
            
        }
    }
}


namespace Automaton.core
{
    public class AutomatonBase
    {
        private Field Field;
        public bool IsStarted { get; private set; }

        public AutomatonBase(int cols, int rows)
        {
            Field = new Field(cols, rows);
            IsStarted = false;
        }

        public bool ChangeStatus()
        {
            IsStarted = !IsStarted;
            return IsStarted;
        }
        
        public Cells NextGeneration()
        {
            return IsStarted ? new Cells(Field.NextGeneration()) : new Cells(Field.Data);
        }

        public Cells Generate()
        {
            return new Cells(Field.Generate());
        }

        public void SetCell(int x, int y)
        {
            Field.SetCell(x, y);
        }
        
    }
}
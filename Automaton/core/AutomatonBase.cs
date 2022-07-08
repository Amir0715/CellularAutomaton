namespace Automaton.core
{
    public class AutomatonBase
    {
        private static AutomatonBase _instance;
        private Field Field;
        
        public bool IsStarted { get; private set; }

        private AutomatonBase(int cols, int rows)
        {
            Field = new Field(cols, rows);
            IsStarted = false;
        }
        
        public static AutomatonBase GetInstance(int cols = 0, int rows = 0)
        {
            return _instance ??= new AutomatonBase(cols, rows);
        }
        
        public bool ChangeStatus()
        {
            IsStarted = !IsStarted;
            return IsStarted;
        }
        
        public Cells NextGeneration(Cells req)
        {
            Field.Data = req.Data;
            return IsStarted ? new Cells(Field.NextGeneration()) : new Cells(Field.Data);
        }

        public Cells Generate(int cols, int rows)
        {
            return new(Field.Generate(cols, rows));
        }

        public void SetCell(int x, int y)
        {
            Field.SetCell(x, y);
        }

        public Cells Clear()
        {
            return new(Field.Clear());
        }
        
    }
}
namespace Automaton.core
{
    public class AutomatonBase
    {
        private Field Field;
        public bool IsStarted { get; private set; }

        public AutomatonBase(int cols, int rows)
        {
            Field = new Field(cols, rows);
        }

        public void Start()
        {
            if (!IsStarted)
                IsStarted = true;
        }

        public void Stop()
        {
            if (IsStarted)
                IsStarted = false;
        }

        public Cell[][] NextGeneration()
        {
            return IsStarted ? Field.NextGeneration() : Field.Data;
        }

        public Cell[][] Generate()
        {
            return Field.Generate();
        }

        public void SetCell(int x, int y)
        {
            Field.SetCell(x, y);
        }
        
    }
}
namespace Automaton.core
{
    public class Automaton
    {
        private Field _field;
        public bool IsStarted { get; private set; }

        public Automaton(int cols, int rows)
        {
            _field = new Field(cols, rows);
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
            return IsStarted ? _field.NextGeneration() : _field.Data;
        }

        public Cell[][] Generate()
        {
            return _field.Generate();
        }
        
        
    }
}
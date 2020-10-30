namespace Automaton.core
{
    public class Automaton
    {
        private Field _field;
        public bool IsStarted { get; private set; }
        
        public Automaton(int width, int height)
        {
            _field = new Field(width, height);
            
        }
        public void Start()
        {
            if (!IsStarted)
                IsStarted = true;
        }

        public void Stop()
        {
            if (IsStarted)
            {
                IsStarted = false;
            }
        }
        public Cell[][] NextGeneration()
        {
            if (IsStarted)
            {
                return _field.NextGeneration();
            }
            else
            {
                return null;
            }
        }

        public void Generate()
        {
            _field.Generate();
        }
        
    }
}
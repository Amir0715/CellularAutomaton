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
            if (IsStarted == false)
                IsStarted = true;
            
        }

        public void NextGeneration()
        {
            if (IsStarted)
            {
                _field.NextGeneration();
            }
        }
    }
}
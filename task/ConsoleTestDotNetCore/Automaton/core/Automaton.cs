namespace Automaton.core
{
    public class Automaton
    {
        private Field _field;
        public bool isStarted { get; private set; }

        public Automaton(int width, int height)
        {
            _field = new Field(width, height);
        }

        public void Start()
        {
            if (isStarted == false)
                isStarted = true;
            
        }

        public void NextGeneration()
        {
            if (isStarted)
            {
                _field?.NextGeneration();
            }
        }
    }
}
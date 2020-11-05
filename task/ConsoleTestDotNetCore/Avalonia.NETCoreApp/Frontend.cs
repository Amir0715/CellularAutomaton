using Automaton.core;

namespace Avalonia.NETCoreApp
{
    public class Frontend
    {
        private AutomatonBase Automaton ;
        private static Frontend _instance;
        private Frontend(int cols, int rows)
        {
            Automaton = new AutomatonBase(cols,rows);
            
            Generate();
        }

        public static Frontend GetInstance(int cols=0, int rows=0)
        {
            return _instance ??= new Frontend(cols, rows);
        }
        
        public void Start()
        {
            Automaton.Start();
        }

        public void Stop()
        {
            Automaton.Stop();
        }
        
        public Cell[][] GetNextGeneration()
        {
            return Automaton.NextGeneration();
        }

        public Cell[][] Generate()
        {
            return Automaton.Generate();
        }

        public void SetCell(int x, int y)
        {
            Automaton.SetCell(x,y, new Cell());
        }

    }
}
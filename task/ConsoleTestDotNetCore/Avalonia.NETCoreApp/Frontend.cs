using Automaton.core;

namespace Avalonia.NETCoreApp
{
    public class Frontend
    {
        private AutomatonBase Automaton;
        private static Frontend _instance;
        public Cell[][] Data { get; private set; }
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
        
        public void NextGeneration()
        {
            Data = Automaton.NextGeneration();
        }

        public void NextStep()
        {
            Start();
            NextGeneration();   
            Stop();
        }

        public void Generate()
        {
            Data =  Automaton.Generate();
        }

        public void SetCell(int x, int y)
        {
            Automaton.SetCell(x,y, new Cell());
        }
    }
}
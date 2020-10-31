using System;
using Automaton.core;
using Avalonia.Media.Imaging;
using Avalonia.Controls;
using AvaloniaUI;
using Automaton = Automaton.core.Automaton;
using Image = Avalonia.Controls.Image;

namespace Avalonia.NETCoreApp
{
    public class Frontend
    {
        private global::Automaton.core.Automaton _automaton;
        private static Frontend instance;
        private Frontend(int cols, int rows)
        {
            _automaton = new global::Automaton.core.Automaton(cols,rows);
            
            Generate();
        }

        public static Frontend GetInstance(int cols=0, int rows=0)
        {
            if (instance == null)
            {
                instance = new Frontend(cols, rows);
            }

            return instance;
        }
        
        public void Start()
        {
            _automaton.Start();
        }

        public void Stop()
        {
            _automaton.Stop();
        }
        
        public Cell[][] GetNextGeneration()
        {
            return _automaton.NextGeneration();
        }

        public Cell[][] Generate()
        {
            return _automaton.Generate();
        }
        
    }
}
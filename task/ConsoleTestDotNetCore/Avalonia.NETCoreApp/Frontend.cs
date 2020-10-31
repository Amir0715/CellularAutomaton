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
        
        public Frontend(int cols, int rows)
        {
            _automaton = new global::Automaton.core.Automaton(cols,rows);
            Generate();
        }

        public Cell[][] GetNextGeneration()
        {
            _automaton.Start();
            return _automaton.NextGeneration();
            _automaton.Stop();
        }

        public Cell[][] Generate()
        {
            return _automaton.Generate();
        }
        
    }
}
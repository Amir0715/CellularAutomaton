using System;
using System.Threading.Tasks;
using Xunit;
using Automaton.core;
using Xunit.Abstractions;

namespace Automaton.Test
{
    public class TaskManagerTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TaskManagerTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }
    }
}
using System;
using Automaton.core;
using Xunit;

namespace Automaton.Test
{
    public class CellTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestValue(bool value)
        {
            var cell = new Cell {IsAlive = value};
            Assert.NotNull(cell);
            Assert.Equal(cell.IsAlive, value);
        }
    }
}
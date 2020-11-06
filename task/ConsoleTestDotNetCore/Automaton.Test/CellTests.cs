using System;
using Automaton.core;
using Xunit;

namespace Automaton.Test
{
    public class CellTests
    {

        [Fact]
        public void TestCell()
        {
            Assert.NotSame(new Cell(), new Cell());
        }
        
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestIsAlive(bool value)
        {
            var cell = new Cell {IsAlive = value};
            Assert.NotNull(cell);
            Assert.Equal(cell.IsAlive, value);
        }

        [Fact]
        public void TestValue()
        {
            var cell = new Cell {Value = 100};
            Assert.Equal(cell.Value, 100);
        }

        [Fact]
        public void TestGenerate()
        {
            var cell = new Cell();
            Random r = new Random();
            cell.Generate(r);
            Assert.IsType<float>(cell.Value);
            Assert.IsType<bool>(cell.IsAlive);
        }

        [Fact]
        public void TestIncrement()
        {
            var cell = new Cell {Value = 10, IsAlive = false};
            cell.Increment();
            Assert.Equal(cell.Value, 11);
            Assert.Equal(cell.IsAlive,true);
        }

        [Fact]
        public void TestDecrement()
        {
            var cell = new Cell {Value = 1, IsAlive = true};
            cell.Decrement();
            Assert.Equal(cell.Value, 0);
            Assert.Equal(cell.IsAlive,false);
        }

        [Fact]
        public void TestNumberOfNeighbors()
        {
            var cell = new Cell() {Value = 1, IsAlive = true, NumberOfNeighbors = 2};
            Assert.Equal(cell.NumberOfNeighbors, 2);
        }

        [Fact]
        public void TestNothing()
        {
            var cell = new Cell() {Value = 1, IsAlive = true, NumberOfNeighbors = 2};
            cell.Nothing();
            Assert.Equal(cell.Value, 1);
            Assert.Equal(cell.IsAlive ,true);
            Assert.Equal(cell.NumberOfNeighbors, 2);
        }

        [Theory]
        [InlineData(true, 0, false)]
        [InlineData(true, 1, false)]
        [InlineData(true, 2, true)]
        [InlineData(true, 3, true)]
        [InlineData(true, 4, false)]
        [InlineData(false, 0,false)]
        [InlineData(false, 1,false)]
        [InlineData(false, 2,false)]
        [InlineData(false, 3,true)]
        [InlineData(false, 4,false)]
        public void TestLife(object alive, object numbers, object expected)
        {
            var cell = new Cell() {NumberOfNeighbors = (int)numbers, IsAlive = (bool)alive};
            cell.Life();
            Assert.Equal(cell.IsAlive,expected);
        }
    }
}
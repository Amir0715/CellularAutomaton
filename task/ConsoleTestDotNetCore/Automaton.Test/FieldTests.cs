using Automaton.core;
using Xunit;

namespace Automaton.Test
{
    public class FieldTests
    {
        [Fact]
        public void TestGenerate()
        {
            var f = new Field(3,3);
            var data = f.Generate();
            foreach (var VARIABLE in data)
            {
                Assert.NotNull(VARIABLE);
            }
        }

        [Fact]
        public void TestNextGeneration()
        {
            var f = new Field(3, 3);
            var data = new Cell[3][];
            for (var i = 0; i < 3; i++)
            {
                data[i] = new Cell[3];
                for (var j = 0; j < 3; j++)
                {
                    data[i][j] = new Cell();
                }
            }
            foreach (var VARIABLE in data[1])
            {
                VARIABLE.IsAlive = true;
            }
            f.Data = data;
            var data2 = f.NextGeneration();
            for (int i = 0; i < 3; i++)
            {
                Assert.True(data2[i][1].IsAlive); 
            }
        }

        [Fact]
        private void TestUpdateNumbersOfNeighbors()
        {
            var f = new Field(3, 3);
            var data = new Cell[3][];
            for (var i = 0; i < 3; i++)
            {
                data[i] = new Cell[3];
                for (var j = 0; j < 3; j++)
                {
                    data[i][j] = new Cell();
                }
            }
            foreach (var VARIABLE in data[1])
            {
                VARIABLE.IsAlive = true;
            }
            f.Data = data;
            var data2 = f.NextGeneration();
            var t = new int[] {3,2,3};
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(t[i],data2[i][1].NumberOfNeighbors); 
            }
        }

        [Fact]
        public void TestSetCell()
        {
            var f = new Field(3,3);
            f.Data[1][1].IsAlive = true;
            f.SetCell(1,1);
            Assert.False(f.Data[1][1].IsAlive);
        }
    }
}
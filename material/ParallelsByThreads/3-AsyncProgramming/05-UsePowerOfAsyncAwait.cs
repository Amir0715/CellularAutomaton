using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ParallelsByThreads
{
    public struct Vector<T>
    {
        private T[] _items;

        public int Length => this._items.Length;

        public Vector(int x)
        {
            if (x < 0)
            {
                throw new ArgumentException("Размерность не может быть отрицательной");
            }
            this._items = new T[x];
        }

        public Vector(params T[] values) { this._items = values; }

        public T this[int index] => this._items[index];
    }

    public struct Matrix<T> : IEnumerable<Vector<T>>
    {
        private T[] _cells;

        public int LinesCount { get; }

        public int ColumnsCount { get; }

        public Matrix(int linesCount, int columnsCount)
        {
            if (linesCount < 0 || columnsCount < 0)
            {
                throw new ArgumentException("Размерность не может быть отрицательной");
            }
            this.LinesCount = linesCount;
            this.ColumnsCount = columnsCount;
            this._cells = new T[linesCount * columnsCount];
        }

        public Matrix(int linesCount, int columnsCount, params T[] values) : this(linesCount, columnsCount)
        {
            if (values.Length != linesCount * columnsCount)
            {
                throw new ArgumentException("Неверное количество переданных элементов матрицы");
            }
            this._cells = values;
        }

        public Vector<T> this[int index] => this.Line(index);

        public T this[int lineIndex, int columnIndex] => this[lineIndex][columnIndex];

        public Vector<T> Line(int index)
        {
            return new Vector<T>(
                this._cells[new Range(index * this.ColumnsCount, index * this.ColumnsCount + this.ColumnsCount)]
            );
        }

        public IEnumerator<Vector<T>> GetEnumerator()
        {
            for (var i = 0; i < this.LinesCount; ++i)
            {
                yield return this.Line(i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            this.GetEnumerator();
    }

    public static class MatrixAsyncExtensions
    {
        public static async Task MeanAsync(this Vector<double> @this, ChannelWriter<double> channelWriter)
        {
            var sum = 0.0;
            for (var i = 0; i < @this.Length; ++i)
            {
                sum += @this[i];
            }
            await channelWriter.WriteAsync(sum / @this.Length);
        }

        public static async Task<double> MeanAsync(this Matrix<double> @this)
        {
            var channel = Channel.CreateBounded<double>(@this.LinesCount);
            var tasks = Task.WhenAll(@this.Select(vector => vector.MeanAsync(channel.Writer)));
            await tasks;
            channel.Writer.Complete();
            var mean = await channel.Reader.MeanAsync();
            return mean;
        }

        public static async Task<double> MeanAsync(this ChannelReader<double> @this)
        {
            var sum = 0.0;
            var i = 0;
            await foreach (var item in @this.ReadAllAsync())
            {
                //Thread.MemoryBarrier();
                sum += item;
                i += 1;
            }
            return sum / i;
        }
    }

    public static class UsePowerOfAsyncAwait
    {
        public static async Task Usage()
        {
            var random = new Random();
            var matrix = new Matrix<double>(
                250,
                40,
                Enumerable.Repeat(0, 10000)
                    .Select(i => 50 * random.NextDouble())
                    .ToArray()
            );
            var mean = await matrix.MeanAsync();
            $"mean={mean}".Print();
        }
    }
}
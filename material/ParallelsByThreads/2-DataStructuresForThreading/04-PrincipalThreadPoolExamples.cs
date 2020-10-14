using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace ParallelsByThreads
{
    /// <summary>
    /// Собственная реализация пула потоков (принципиальная)
    /// </summary>
    public class MyThreadPool : IDisposable
    {
        /// <summary>
        /// Хранилище потоков
        /// </summary>
        private ConcurrentDictionary<Guid, Thread> Pool { get; }

        /// <summary>
        /// Множество отдыхающих потоков
        /// </summary>
        private ConcurrentDictionary<Guid, bool> AvailableThreadsIds { get; }

        private ConcurrentQueue<Action> Tasks { get; }

        private volatile bool _isAlive;

        public bool IsAlive => this._isAlive;

        private void Work()
        {
            var guid = new Guid();
            this.Pool.TryAdd(guid, Thread.CurrentThread);
            this.AvailableThreadsIds.TryAdd(guid, true);
            $"Thread[{guid}]: starting".Print();
            while (this._isAlive)
            {
                if (!this.Tasks.TryDequeue(out var task))
                {
                    Thread.Sleep(500);
                    continue;
                }
                this.AvailableThreadsIds.TryRemove(guid, out _);
                $"Thread[{guid}]: took task".Print();
                task.Invoke();
                this.AvailableThreadsIds.TryAdd(guid, true);
            }
            $"Thread[{guid}]: ending".Print();
            this.Pool.TryRemove(guid, out _);
            this.AvailableThreadsIds.TryRemove(guid, out _);
        }

        public MyThreadPool(int minThreadCount = 4)
        {
            this.Pool = new ConcurrentDictionary<Guid, Thread>();
            this.AvailableThreadsIds = new ConcurrentDictionary<Guid, bool>();
            this.Tasks = new ConcurrentQueue<Action>();
            this._isAlive = true;
            for (var i = 0; i < minThreadCount; ++i)
            {
                $"ThreadPool: add new Thread, count={this.Pool.Count}".Print();
                var thread = new Thread(this.Work);
                thread.Start();
            }
        }

        public void Push(Action task)
        {
            "ThreadPool: pushing task".Print();
            // Добавление задачи
            this.Tasks.Enqueue(task);
            // Критерий увеличения количества потоков
            var needAdd = this.Pool.Count - this.AvailableThreadsIds.Count < this.Pool.Count / 2;
            if (needAdd)
            {
                $"ThreadPool: add new Thread, count={this.Pool.Count}".Print();
                var thread = new Thread(this.Work);
                thread.Start();
            }
            // Критерий уменьшения количества потоков - отсутствует :(
        }

        public void WaitUntilAllTasksDone()
        {
            "ThreadPool: waiting".Print();
            while (this.Tasks.Count != 0)
            {
                Thread.Sleep(5);
            }
        }

        public void Dispose()
        {
            "ThreadPool: cancelling".Print();
            this._isAlive = false;
        }
    }
    
    public class PrincipalThreadPoolExamples
    {
        // Как использовать реализованный пул потоков?
        public static void HowToUse()
        {
            using var threadPool = new MyThreadPool();
            
            Thread.Sleep(10);

            var results = new ConcurrentQueue<double>();

            for (var i = 0; i < 10; i++)
            {
                threadPool.Push(
                    () =>
                    {
                        var random = new Random();
                        results.Enqueue(random.NextDouble());
                    }
                );
            }

            threadPool.WaitUntilAllTasksDone();

            $"ThreadPool is alive: {threadPool.IsAlive}".Print();
            $"{results.ToList().Sum() / 1000}".Print();
        }
    }
}
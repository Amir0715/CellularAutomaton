using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelsByThreads
{
    public static class MoreTaskExamples
    {
        private static async Task MyTask()
        {
            throw new Exception("Hello, here is my exception");
        }

        public static async Task HowToHandleExceptions()
        {
            try
            {
                await MyTask();
            }
            catch (Exception e)
            {
                $"{e.Message}".Print();
            }
        }

        public static async Task HowToWaitForTasks()
        {
            var cts = new CancellationTokenSource();
            var t1 = Task.Run(
                () =>
                {
                    Thread.Sleep(10);
                    "Task 1".Print();
                },
                cts.Token
            );
            var t2 = Task.Run(
                () =>
                {
                    Thread.Sleep(10);
                    "Task 2".Print();
                },
                cts.Token
            );
            var t3 = Task.Run(
                () =>
                {
                    Thread.Sleep(10);
                    "Task 3".Print();
                },
                cts.Token
            );

            await Task.WhenAll(t1, t2, t3);
            //await Task.WhenAny(t1, t2, t3);
            //cts.Cancel();
        }
    }
}
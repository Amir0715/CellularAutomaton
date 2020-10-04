using System;
using System.Threading;

namespace Parprog.Threading
{
    public class SyncErrors
    {
        public static void ExampleRaceCondition()
        {
            var x = 0;
            for (var i = 1; i < 5; i++)
            {
                var thread = new Thread(Increment);
                thread.Name = $"Thread {i}";
                thread.Start(i * 10);
            }

            void Increment(object data)
            {
                var timeout = (int) data;
                for (var i = 1; i < 9; i++)
                {
                    Console.WriteLine("{0}: {1}", Thread.CurrentThread.Name, x);
                    x++;
                    Thread.Sleep(timeout);
                }
            }
        }

        public static void ExampleDeadlock()
        {
            //TODO
        }
    }
}
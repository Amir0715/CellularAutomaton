using System;
using System.Threading;

namespace Parprog.Threading
{
    public static class BasicThreading
    {
        public static void ExampleCurrentThreadInfo()
        {
            var thread = Thread.CurrentThread;
            Console.WriteLine($"Имя потока: {thread.Name}");
            thread.Name = "Метод Main";
            Console.WriteLine($"Имя потока: {thread.Name}");
            Console.WriteLine($"Запущен ли поток: {thread.IsAlive}");
            Console.WriteLine($"Приоритет потока: {thread.Priority}");
            Console.WriteLine($"Статус потока: {thread.ThreadState}");
        }

        public static void ExampleCreateAndStartThread()
        {
            var thread = new Thread(new ThreadStart(Count));
            Console.WriteLine($"Статус потока: {thread.ThreadState}");
            thread.Start();
            Console.WriteLine($"Статус потока: {thread.ThreadState}");
            for (var j = 100; j < 110; j++)
            {
                Console.WriteLine(j);
                Thread.Sleep(10);
            }
            thread.Join();
            Console.WriteLine($"Статус потока: {thread.ThreadState}");

            void Count()
            {
                for (var i = 0; i < 10; i++)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(5);
                }
            }
        }

        public static void ExampleParametrizedThread()
        {
            var thread = new Thread(new ParameterizedThreadStart(Count));
            thread.Start(10);
            thread.Join();

            void Count(object data)
            {
                var offset = (int) data;
                for (var i = offset; i < 10; i++)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(5);
                }
            }
        }

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

        
    }
}
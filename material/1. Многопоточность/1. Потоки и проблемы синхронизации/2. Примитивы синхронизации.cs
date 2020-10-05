using System;
using System.Threading;

namespace Parprog.Threading
{
    public class SyncPrimitives
    {
        public static void ExampleLocker()
        {
            var x = 0;
            var locker = new object();
            for (var i = 1; i < 5; i++)
            {
                var thread = new Thread(Increment);
                thread.Name = $"Thread {i}";
                thread.Start(i * 10);
            }
            Console.ReadKey();

            void Increment(object data)
            {
                var timeout = (int) data;
                for (var i = 1; i < 9; i++)
                {
                    lock (locker)
                    {
                        Console.WriteLine("{0}: {1}", Thread.CurrentThread.Name, x);
                        x++;
                    }
                    Thread.Sleep(timeout);
                }
            }
        }

        public static void ExampleMutex()
        {
            var x = 0;
            var mutex = new Mutex();
            for (var i = 1; i < 5; i++)
            {
                var thread = new Thread(Increment);
                thread.Name = $"Thread {i}";
                thread.Start(i * 10);
            }
            Console.ReadKey();

            void Increment(object data)
            {
                var timeout = (int) data;
                for (var i = 1; i < 9; i++)
                {
                    mutex.WaitOne();
                    Console.WriteLine("{0}: {1}", Thread.CurrentThread.Name, x);
                    x++;
                    mutex.ReleaseMutex();
                    Thread.Sleep(timeout);
                }
            }
        }

        public static void ExampleSemaphore()
        {
            using var semaphore = new Semaphore(0, 3);
            for (var i = 1; i <= 9; i++)
            {
                var thread = new Thread(Business);
                thread.Name = $"{i}";
                thread.Start(thread);
            }
            Console.WriteLine("Открыли семафор");
            Console.WriteLine($"Было мест: {semaphore.Release(3)}");

            void Business(object arg)
            {
                var self = (Thread) arg;
                semaphore.WaitOne();
                Console.WriteLine($"{self.Name} begin");
                Thread.Sleep(100);
                Console.WriteLine($"{self.Name} do");
                Thread.Sleep(1000);
                Thread.Sleep(100);
                Console.WriteLine($"{self.Name} end");
                Console.WriteLine($"Осталось мест: {semaphore.Release() + 1}");
            }

            Console.ReadKey();
        }
        
        public static void ExampleDeadlock()
        {
            var object1 = new object();
            var object2 = new object();
            void LockMe()
            {
                lock (object1)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Locked 1");
                    lock (object2)
                    {
                        Console.WriteLine("Locked then 2");
                    }
                }
            }
            
            new Thread(LockMe).Start();
            lock (object2)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Locked 2");
                lock (object1)
                {
                    Console.WriteLine("Locked then 1");
                }
            }
        }
    }
}
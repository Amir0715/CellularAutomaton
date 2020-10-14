using System;
using System.Collections.Generic;
using System.Threading;

namespace ParallelsByThreads
{
    public class DeadlockExample
    {
        public static void DeadlockByLockers()
        {
            // Создаём два объекта-локера
            var object1 = new object();
            var object2 = new object();
            void LockMe()
            {
                // Захватываем первый
                lock (object1)
                {
                    // Подождём, пока другой поток залочится
                    Thread.Sleep(1000);
                    Console.WriteLine("Locked 1");
                    // Затем захватываем второй
                    lock (object2)
                    {
                        Console.WriteLine("Locked then 2");
                    }
                }
            }
            
            new Thread(LockMe).Start();
            // Здесь же наоборот: захватываем второй
            lock (object2)
            {
                // Подождём, пока другой поток залочится
                Thread.Sleep(1000);
                Console.WriteLine("Locked 2");
                // И затем первый
                lock (object1)
                {
                    Console.WriteLine("Locked then 1");
                }
            }
            
            // Программа зависла, дальнейшие строки кода никогда не выполнятся
        }

        public static void DeadlockByJoin()
        {
            // Функция принимает в себя ссылки на другие потоки и просто начинает ждать,
            // пока они завершатся
            void Worker(object data)
            {
                var threads = data as List<Thread>;
                Thread.Sleep(20);
                foreach (var thread in threads)
                {
                    thread.Join();
                }
            }
            
            var threadA = new Thread(Worker);
            var threadB = new Thread(Worker);
            var threadC = new Thread(Worker);
            
            threadA.Start(new List<Thread>(){threadB, threadC});
            threadB.Start(new List<Thread>(){threadA, threadC});
            threadC.Start(new List<Thread>(){threadA, threadB});
            
            // Основной поток никогда не дождётся этих трёх потоков

            threadA.Join();
            threadB.Join();
            threadC.Join();
        }
    }
}
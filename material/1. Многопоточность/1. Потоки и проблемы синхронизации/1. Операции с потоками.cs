using System;
using System.Threading;

namespace Parprog.Threading
{
    /// <summary>
    /// Примеры, показывающие основные способы работы с потоками (класс Thread).
    /// </summary>
    public static class Threads
    {
        /// <summary>
        /// Получение информации о текущем потоке исполнения.
        /// </summary>
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

        /// <summary>
        /// Создание и запуск нового потока с функцией, не требующей аргументов.
        /// </summary>
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

        /// <summary>
        /// Создание и запуск потока с функцией, требующей аргументы.
        /// </summary>
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
    }
}
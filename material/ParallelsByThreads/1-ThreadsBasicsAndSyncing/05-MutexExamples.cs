using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace ParallelsByThreads
{
    public class MutexExamples
    {
        // Починим Race Condition с помощью примитива Mutex. Здесь без разницы, можно было бы
        // и lock обойтись
        public static void ChangeAfterCondition()
        {
            var x = 10;
            var result = 0;
            // Создали mutex в разделяемой памяти
            var mutex = new Mutex();

            void Add()
            {
                // Закрыли mutex
                mutex.WaitOne();
                if (x == 10)
                {
                    $"{Thread.CurrentThread.Name}: x={x}".Print();
                    Thread.Sleep(10);
                    result = x * 3 + x;
                }
                // Открыли mutex
                mutex.ReleaseMutex();
            }

            var thread = new Thread(Add)
            {
                Name = "Outer"
            };
            thread.Start();
            Thread.Sleep(5);
            // Здесь аналогично запираем mutex
            mutex.WaitOne();
            x = 1;
            // А здесь отпираем
            mutex.ReleaseMutex();
            thread.Join();
            $"Current: x={x}, result={result}".Print();
        }

        public static void ReallyUsefulMutex()
        {
            var random = new Random();
            var mutex = new Mutex();
            var currentConfigPath = "";

            void GameUI()
            {
                for (var i = 0; i < 20; ++i)
                {
                    Thread.Sleep(random.Next(20));
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    currentConfigPath = new string(
                        Enumerable
                            .Repeat(chars, 15)
                            .Select(s => s[random.Next(s.Length)])
                            .ToArray()
                    );
                }
            }

            var thread = new Thread(GameUI);
            thread.Start();
            
            for (var i = 0; i < 10; i++)
            {
                Thread.Sleep(random.Next(20));
                try
                {
                    mutex.WaitOne();
                    using var reader = new StreamReader(currentConfigPath);
                    mutex.ReleaseMutex();
                    reader.ReadToEnd().Print();
                }
                catch (Exception e)
                {
                    $"File access error: {currentConfigPath}. {e.Message}".Print();
                    mutex.ReleaseMutex();
                }
            }
            
            thread.Join();
        }
    }
}
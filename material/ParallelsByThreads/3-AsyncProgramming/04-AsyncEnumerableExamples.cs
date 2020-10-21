using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelsByThreads
{
    public static class AsyncEnumerableExamples
    {
        public static async Task BasicUsage()
        {
            var t1 = Task.Run(async () => await PrintNumbers());
            var t2 = Task.Run(async () => await PrintNumbers());
            Thread.Sleep(1500);
            Console.WriteLine("Main thread is coming");
            await Task.WhenAll(t1,t2);
        }
        
        private static async Task PrintNumbers()
        {
            // Вычитываем их асинхронной последовательности
            await foreach (var (value, delay) in GetNumbersAsync())
            {
                // Записываем результат и время ожидания
                Console.WriteLine($"out: {value}, delayed: {delay}");
            }
        }

        private static async IAsyncEnumerable<(int, int)> GetNumbersAsync()
        {
            // Возвращаем коллекцию из чисел от 0 до 9 с рандомной паузой
            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var delay = random.Next(500) + 500;
                await Task.Delay(delay);
                yield return (i, delay);
            }
        }
    }
}
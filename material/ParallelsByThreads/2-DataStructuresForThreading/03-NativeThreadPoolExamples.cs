using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace ParallelsByThreads
{
    public class NativeThreadPoolExamples
    {
        // Пример использования нативного .NET'овского пула потоков (ThreadPool)
        public static void BasicThreadPoolUsage()
        {
            // Будем в нескольких потоках брать по кусочку данных из массива data и находить сумму,
            // затем складывать результаты в ThreadSafe-хранилище results
            const int count = 10000;
            const int max = 50;
            const int limit = 100;
            var random = new Random();
            var data = new int[count];
            for (var i = 0; i < count; ++i)
            {
                data[i] = random.Next(max);
            }
            var results = new ConcurrentBag<int>();

            // Просто считает сумму кусочка данных массива
            void Worker(object arg)
            {
                var offset = (int) arg;
                var sum = 0;
                for (var i = offset; i < offset + limit; i++)
                {
                    sum += data[i];
                }
                results.Add(sum);
            }
            
            // Под "задачей" будем понимать одно исполнение функции Worker
            // Обязательно нужно знать объём работы - сколько задач нужно выполнить
            const int taskCount = count / limit;
            for (var i = 0; i < taskCount; i++)
            {
                // Запуск задачи на выполнение и передача ей аргумента
                ThreadPool.QueueUserWorkItem(Worker, i + limit);
            }
            // Нам надо дождаться, пока все задачи не будут выполнены. Для этого мы запоминали
            // их общее количество
            while (results.Count != taskCount)
            {
                Thread.Sleep(10);
            }

            // А теперь можно просто продолжить расчёт
            var mean = results.Sum() / taskCount / limit;

            // Так как Random генерирует случайные числа равомерно распределённые от 0 до max=50
            // среднее получаем в районе 25 
            $"Mean: {mean}".Print();
        }
    }
}
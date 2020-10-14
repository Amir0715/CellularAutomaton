using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ParallelsByThreads
{
    public class ConcurrentDictionaryExamples
    {
        // Как можно реализовать кэш для нескольких потоков?
        public static void ConcurrentDictionaryAsCache()
        {
            // С помощью Thread Safe реализации словаря, конечно же
            var cache = new ConcurrentDictionary<string, (long Value, DateTime SetupTime)>();

            // Эта функция будет очищать протухшие ключи
            void Cleaner(object data)
            {
                // Эта функция не имеет собственного "окончания", очистка кэша происходит
                // потенциально бесконечно. Поэтому для прерывания работы потока нужно использовать
                // флаги. Например, CancellationToken
                var cancellationToken = (CancellationToken) data;
                while (!cancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(1000);
                    // Достаточно наивная реализация как кэша, так и очистки, но ладно
                    foreach (var (key, value) in cache.ToArray())
                    {
                        var keyTime = value.SetupTime;
                        var currentTime = DateTime.Now;
                        var duration = currentTime - keyTime;
                        if (duration.TotalMilliseconds > 1000)
                        {
                            cache.TryRemove(key, out _);
                        }
                    }
                }
            }

            // Эта функция будет использовать кэш при вычислениях
            void AbsurdSummator()
            {
                // Пытаемся возвести случайные числа в степень друг друга, кэшируя вычисления
                var random = new Random();
                const int max = 10;
                for (var i = 0; i < 10000; ++i)
                {
                    // Что возводим
                    var a = random.Next(max);
                    // Во что возводим
                    var b = random.Next(max);
                    // Паттерн ключа
                    var key = $"{a}{b}";
                    // Есть ли в кэше
                    if (cache.TryGetValue(key, out var pair))
                    {
                        $"{pair.Value}".Print();
                    }
                    else
                    {
                        // Если нет, то сами вычисляем и кладём в кэш
                        var result = Convert.ToInt32(Math.Pow(a, b));
                        cache.TryAdd(key, (result, DateTime.Now));
                    }
                }
            }

            // Пару сумматоров
            var summator1 = new Thread(AbsurdSummator);
            var summator2 = new Thread(AbsurdSummator);
            // Один очищальщик
            var cleaner = new Thread(Cleaner);
            // И CancellationTokenSource для управления очищальщиком
            var cts = new CancellationTokenSource();

            // Запуск потоков
            summator1.Start();
            summator2.Start();
            cleaner.Start(cts.Token);

            // Сумматоры ждём как обычно
            summator1.Join();
            summator2.Join();
            // Прерывание работы потока через CancellationToken. Почему так - объяснение ниже
            cts.Cancel();
            cleaner.Join();
            // Метод Abort у класса Thread не работает на Unix-системах. Только в Windows.
            // cleaner.Abort();
        }
    }
}
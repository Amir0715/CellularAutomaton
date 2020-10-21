using System.Threading;

namespace ParallelsByThreads
{
    public class AtomicOperationsExamples
    {
        private static int counter = 0;

        // Пример, когда операции являются не атомарными. Например, сложение чисел типа double.
        public static void AtomicAndNoneVariants()
        {
            // Простая функция инкремента
            static void Inc()
            {
                for (var i = 0; i < 100000; i++)
                {
                    counter++; // Операция не атомарная
                    //++counter;    // Операция не атомарная
                    //counter += 1; // Операция не атомарная
                    //Interlocked.Increment(ref counter); // Атомарный инкремент
                    //Interlocked.Add(ref counter, 2);   // Атомарное прибавление
                }
            }

            // Сделаем 4 потока
            var threads = new[] {new Thread(Inc), new Thread(Inc), new Thread(Inc), new Thread(Inc)};
            // Все их запустим
            foreach (var thread in threads)
            {
                thread.Start();
            }
            // И дождёмся завершения
            foreach (var thread in threads)
            {
                thread.Join();
            }
            // Ожидаем в counter 40000
            $"Counter: actual={counter}, expected={100000*threads.Length}".Print();
            // Всё из-за ошибки Read-Modify-Write - неатомарные операции нужно синхронизировать
        }
    }
}
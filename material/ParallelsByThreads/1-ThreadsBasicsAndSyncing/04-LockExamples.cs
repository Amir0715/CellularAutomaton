using System.Threading;

namespace ParallelsByThreads
{
    public static class LockExamples
    {
        // Починим Race Condition с помощью примитива lock
        public static void ChangeAfterCondition()
        {
            var x = 10;
            var result = 0;
            var locker = new object();

            void Add()
            {
                // Обернули наше использование переменной x в lock
                lock (locker)
                {
                    if (x == 10)
                    {
                        $"{Thread.CurrentThread.Name}: x={x}".Print();
                        Thread.Sleep(10);
                        result = x * 3 + x;
                    }
                }
            }

            var thread = new Thread(Add)
            {
                Name = "Outer"
            };
            thread.Start();
            Thread.Sleep(5);
            // Здесь используем тот же locker
            lock (locker)
            {
                x = 1;
            }
            thread.Join();
            // В результате x=1 и result = 40. Как мы и хотели.
            $"Current: x={x}, result={result}".Print();
            // Используя locker, мы предохранились от нежелательных действий других потоков на разделяемую память
            // Но наверняка потеряли в производительности - но об этом позже
        }

        private static double counter = 0;

        // Пример, когда операции являются не атомарными. Например, сложение чисел типа double.
        public static void NonAtomicOperations()
        {
            var locker = new object();
            // Простая функция инкремента
            void Inc()
            {
                for (var i = 0; i < 100000; i++)
                {
                    lock (locker)
                    {
                        counter += 1; // такой инкремент не атомарный
                        // Но locker спасает и в этой ситуации. Другие потоки будут ждать, пока
                        // один из них сделает инкремент
                    }
                }
            }

            var threads = new[] {new Thread(Inc), new Thread(Inc), new Thread(Inc), new Thread(Inc)};
            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            // Ожидаем в counter 40000, ответ будет какой угодно другой
            $"Counter: actual={counter}, expected={100000*threads.Length}".Print();
            // Всё из-за ошибки Read-Modify-Write - неатомарные операции нужно синхронизировать
        }
    }
}
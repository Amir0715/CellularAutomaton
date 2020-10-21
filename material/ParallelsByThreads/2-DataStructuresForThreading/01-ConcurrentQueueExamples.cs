using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ParallelsByThreads
{
    public class ConcurrentQueueExamples
    {
        // Будем использовать очередь для передачи данных между потоками
        public static void CommonQueueInMultithreading()
        {
            // Возьмём обычную очередь
            var queue = new Queue<int>();

            // Просто много раз добавляем в очередь элементы
            void Push()
            {
                for (var i = 0; i < 5000; i++)
                {
                    queue.Enqueue(i);
                }
            }

            // Просто много раз вынимаем из очереди элементы
            void Pop()
            {
                var c = 0;
                for (var i = 0; i < 10000; i++)
                {
                    if (queue.TryDequeue(out _))
                    {
                        c++;
                    }
                }
                $"received count: {c}".ToString().Print();
            }
            
            // Пусть у нас будет два потока добавлять элементы
            var pusher1 = new Thread(Push);
            var pusher2 = new Thread(Push);
            // И один поток вынимать
            var poper = new Thread(Pop);
            
            pusher1.Start();
            pusher2.Start();
            poper.Start();
            
            // И тут вылезает Exception - мы сломали очередь
            // Обычная System.Collection.Generic.Queue<T> не предназначена для использования
            // в нескольких потоках

            pusher1.Join();
            pusher2.Join();
            poper.Join();
        }

        // Теперь возьмём специальную Thread Safe реализацию очереди
        public static void ConcurrentQueueInMultithreading()
        {
            // Возьмём обычную очередь
            var queue = new ConcurrentQueue<int>();

            // Просто много раз добавляем в очередь элементы
            void Push()
            {
                for (var i = 0; i < 5000; i++)
                {
                    queue.Enqueue(i);
                }
            }

            // Просто много раз вынимаем из очереди элементы
            void Pop()
            {
                var c = 0;
                for (var i = 0; i < 10000; i++)
                {
                    if (queue.TryDequeue(out _))
                    {
                        c++;
                    }
                }
                $"received count: {c}".Print();
            }
            
            // Пусть у нас будет два потока добавлять элементы
            var pusher1 = new Thread(Push);
            var pusher2 = new Thread(Push);
            // И один поток вынимать
            var poper = new Thread(Pop);
            
            pusher1.Start();
            pusher2.Start();
            poper.Start();
            
            // Поменяли один класс - всё починилось

            pusher1.Join();
            pusher2.Join();
            poper.Join();
        }
    }
}
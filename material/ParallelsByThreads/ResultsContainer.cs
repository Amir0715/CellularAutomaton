using System;
using System.Collections.Concurrent;

namespace ParallelsByThreads
{
    /// <summary>
    /// Специальный класс, осуществляющий накопление результатов вывода в консоль. Обращение к классу
    /// System.Console является блокирующей операцией, что может повлиять на корректность исполнения примеров.
    /// Из-за использования вызовов консоли напрямую многие эффекты могут быть не видны.
    /// </summary>
    public static class ResultsContainer
    {
        private static ConcurrentQueue<object> Result { get; }

        static ResultsContainer()
        {
            Result = new ConcurrentQueue<object>();
        }

        public static void Attach(object o) =>
            Result.Enqueue(o);

        public static void Print()
        {
            foreach (var item in Result)
            {
                Console.WriteLine(item);
            }
        }
    }
}
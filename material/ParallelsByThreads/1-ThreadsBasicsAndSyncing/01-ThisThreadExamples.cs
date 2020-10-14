using System.Threading;

namespace ParallelsByThreads
{
    public class ThisThreadExamples
    {
        public static void ViewCurrentThreadStats()
        {
            // Получение объекта потока, в котором сейчас выполняется код
            var thread = Thread.CurrentThread;
            // Имя потока по-умолчанию пустое
            $"Имя потока: {thread.Name}".Print();
            // Если нужно, его можно установить
            thread.Name = "Метод Main";
            $"Имя потока: {thread.Name}".Print();
            // Можно узнать, работает ли в данный момент поток, или он уже завершил работу
            $"Запущен ли поток: {thread.IsAlive}".Print();
            // Можно узнать, какой приоритет у потока: от 0 до 4
            $"Приоритет потока: {thread.Priority}".Print();
            // Можно узнать, каков статус потока
            $"Статус потока: {thread.ThreadState}".Print();
        }
    }
}
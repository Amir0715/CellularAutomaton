using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelsByThreads
{
    // Общая статья по асинхронности в .NET: https://habr.com/ru/company/otus/blog/488082/
    // Довольно интересное сравнение C# Task и Go goroutines:
    // https://medium.com/@karl.pickett/benchmarking-a-toy-c-task-vs-a-go-goroutine-is-there-any-difference-248f73f7f7b7
    public static class TaskExamples
    {
        // Принципиальное описание асинхронности
        public static async Task AsyncOperations()
        {
            var start = DateTime.Now;
            $"Before call: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
            var task = Task.Delay(1000);
            //Thread.Sleep(1000 + 200); // 
            $"After call: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
            await task;
            $"After await: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
        }

        private static int _ms = 100; // Меняйте размер этого таймаута - он симулирует деятельность в функциях далее

        // Будем читать и писать в файл синхронно и посмотрим, сколько времени это займёт
        public static void ReadWriteFileNotAsync()
        {
            var start = DateTime.Now;
            string source;
            $"Starting. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
            // Будем работать с достаточно большим файлом.
            using (var reader = new StreamReader(
                "/Users/a17771359/Education/mephi-parprog-2020/material/ParallelsByThreads/files/Book.txt"
            ))
            {
                $"Before reading. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                // Считываем файл в строку
                source = reader.ReadToEnd();
                $"After reading. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                // Эмулируем другую работу
                Thread.Sleep(_ms);
                $"After delay. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
            }
            // Эмулируем работу
            Thread.Sleep(_ms);
            // Записываем в другой файл то же самое - главое, что много
            using (var writer = new StreamWriter(
                "/Users/a17771359/Education/mephi-parprog-2020/material/ParallelsByThreads/files/Written.txt",
                false
            ))
            {
                $"Before write. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                // Записываем строку в файл
                writer.Write(source);
                $"After write. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                // Эмулируем другую работу
                Thread.Sleep(_ms);
                $"After delay. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
            }
            $"The end. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
        }

        // Типичное применение асинхронности
        public static async Task ReadWriteFileAsync()
        {
            string source;
            var start = DateTime.Now;
            $"Starting. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
            using (var reader = new StreamReader(
                "/Users/a17771359/Education/mephi-parprog-2020/material/ParallelsByThreads/files/Book.txt"
            ))
            {
                $"Before reading. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                // Создадим задачу на выполнение в пуле потоков
                var readTask = reader.ReadToEndWrapperAsync(start);
                // Эмулируем другую работу
                await Task.Delay(_ms);
                $"After read and delay. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                // Получаем содержимое файла в тот момент, когда это нужно
                source = await readTask;
                $"After awaiting. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
            }
            // Эмулируем работу
            await Task.Delay(_ms);
            using (var writer = new StreamWriter(
                "/Users/a17771359/Education/mephi-parprog-2020/material/ParallelsByThreads/files/Written.txt",
                false
            ))
            {
                $"Before writing. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                // Создаём задачу на запись файла - она уже начала выполняться
                var writeTask = writer.WriteWrapperAsync(source, start);
                // Эмулируем работу
                await Task.Delay(_ms);
                $"After write and delay. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                // Дожидаемся конца записи в файл тогда, когда уже вся другая работа сделана
                await writeTask;
                $"After awaiting. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
            }
            $"The end. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
        }

        // Функция-обёртка, чтобы добавить логирование операции
        public static async Task<string> ReadToEndWrapperAsync(this StreamReader streamReader, DateTime start)
        {
            $"ReadToEnd. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
            return await streamReader.ReadToEndAsync();
        }

        // Функция-обёртка, чтобы добавить логирование операции
        public static async Task WriteWrapperAsync(this StreamWriter streamWriter, string? value, DateTime start)
        {
            $"Write. Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
            await streamWriter.WriteAsync(value);
        }
    }
}
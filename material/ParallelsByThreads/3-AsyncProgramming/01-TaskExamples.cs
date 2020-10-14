using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelsByThreads
{
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

        private static int _ms = 100; // Меняйте размер этого таймаута - он симулирует нагрузку в функциях
        
        // Типичное применение асинхронности
        public static async Task ReadWriteFileAsync()
        {
            string configSource;
            var start = DateTime.Now;
            $"1 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
            using (var reader = new StreamReader(
                "/Users/a17771359/Education/mephi-parprog-2020/material/ParallelsByThreads/files/Book.txt"
            ))
            {
                $"2 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                var readTask = reader.ReadToEndAsync();
                await Task.Delay(_ms);
                $"3 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                configSource = await readTask;
                $"4 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
            }
            await Task.Delay(_ms);
            using (var writer = new StreamWriter(
                "/Users/a17771359/Education/mephi-parprog-2020/material/ParallelsByThreads/files/Written.txt",
                false
            ))
            {
                $"5 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                var writeTask = writer.WriteAsync(configSource);
                await Task.Delay(_ms);
                $"6 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                await writeTask;
                $"7 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
            }
            $"8 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
        }

        // Типичное применение асинхронности
        public static void ReadWriteFileNotAsync()
        {
            var start = DateTime.Now;
            string configSource;
            $"1 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
            using (var reader = new StreamReader(
                "/Users/a17771359/Education/mephi-parprog-2020/material/ParallelsByThreads/files/Book.txt"
            ))
            {
                $"2 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                configSource = reader.ReadToEnd();
                $"3 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                Thread.Sleep(_ms);
                $"4 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
            }
            Thread.Sleep(_ms);
            using (var writer = new StreamWriter(
                "/Users/a17771359/Education/mephi-parprog-2020/material/ParallelsByThreads/files/Written.txt",
                false
            ))
            {
                $"5 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                writer.Write(configSource);
                $"6 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
                Thread.Sleep(_ms);
                $"7 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                    .Print();
            }
            $"8 Timing: {(DateTime.Now - start).TotalMilliseconds}. ThreadId={Thread.CurrentThread.ManagedThreadId}"
                .Print();
        }
    }
}
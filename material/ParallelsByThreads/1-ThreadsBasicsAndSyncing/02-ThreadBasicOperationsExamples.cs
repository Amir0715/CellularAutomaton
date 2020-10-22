using System.Threading;

namespace ParallelsByThreads
{
    public class ThreadBasicOperationsExamples
    {
        private static void SelfPrinter()
        {
            for (var i = 0; i < 10; ++i)
            {
                $"{Thread.CurrentThread.Name}-{i}".Print();
            }
        }
        
        // Пример создания, запуска и ожидания завершения потока, выполняющего функцию без аргументов
        public static void CreateAndStartThread()
        {
            // Cоздание первого потока и задание ему имени в блоке инициализации
            var threadMicrosoft = new Thread(SelfPrinter)
            {
                Name = "Microsoft"
            };
            // Создание второго потока и задание ему имени
            var threadApple = new Thread(SelfPrinter)
            {
                Name = "Apple"
            };
            // Запуск потоков на исполнение
            threadMicrosoft.Start();
            threadApple.Start();

            // Теперь необходимо в основном потоке дождаться, пока выполняться побочные
            // Для этого используется метод Join - он блокирует исполнение текущего потока
            // до тех пор, пока не будет завершён тот, у которого Join был вызван.
            threadMicrosoft.Join();
            threadApple.Join();
            
            // В итоге видим, что результаты выводятся в консоль вразнобой - потоки действительно
            // работают параллельно
        }
        
        private static void AnyPrinter<T>(T item)
        {
            for (var i = 0; i < 10; ++i)
            {
                $"{Thread.CurrentThread.Name}-{i}: {item}".Print();
            }
        }

        // Пример создания, запуска и ожидания завершения потока, выполняющего функцию с аргументом
        public static void CreateAndStartParametrizedThread()
        {
            var threadMicrosoft = new Thread(AnyPrinter)
            {
                Name = "Microsoft"
            };
            var threadApple = new Thread(AnyPrinter)
            {
                Name = "Apple"
            };
            // Чтобы передать значение в функцию AnyPrinter достаточно это значение передать в
            // метод Start
            threadMicrosoft.Start("Nadella");
            threadApple.Start("?");

            threadMicrosoft.Join();
            threadApple.Join();
        }
    }
}
using System.Threading;

namespace ParallelsByThreads
{
    public class RaceConditionExamples
    {
        // Также такой тип ошибки называют Check-Then-Act
        public static void ChangeAfterCondition()
        {
            // Построим race condition вокруг переменной x: будем использовать её значение
            // в разных потоках
            var x = 10;
            var result = 0;

            void Add()
            {
                // Ожидаем после этой строки, что дальше в блоке if переменная x равна 10
                if (x == 10)
                {
                    // Выведем значение, которое только что проверили: действительно x=10
                    $"{Thread.CurrentThread.Name}: x={x}".Print();
                    // Сэмулируем какую-нибудь деятельность - подождём 10 милисекунд
                    Thread.Sleep(10);
                    // Ожидаем 10*3+10=40
                    result = x * 3 + x;
                }
            }

            // Создаём поток с функцией, вычисляющей result
            var thread = new Thread(Add)
            {
                Name = "Outer"
            };
            thread.Start();
            // Подождём немного, чтобы в другом потоке исполнение прошло через if, но вычисление result
            // не было ещё осуществлено
            Thread.Sleep(5);
            // Изменяем значение x с 10 на 1
            x = 1;
            // Ждём завершения вычислений
            thread.Join();
            // В результате x=1 и result = 4. Хотя в функции Add мы дополнительно проверили, что x=10
            $"Current: x={x}, result={result}".Print();
            // Неаккуратное использование разделяемой между потоками памяти (в данном случае переменной x)
            // влечёт непредсказуемое поведение программы
        }

        private static double counter = 0;

        // Пример, когда операции являются не атомарными. Например, сложение чисел типа double.
        public static void NonAtomicOperations()
        {
            // Простая функция инкремента
            static void Inc()
            {
                for (var i = 0; i < 100000; i++)
                {
                    counter += 1; // такой инкремент не атомарный
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
            // Ожидаем в counter 40000, ответ будет какой угодно другой
            $"Counter: actual={counter}, expected={100000*threads.Length}".Print();
            // Всё из-за ошибки Read-Modify-Write - неатомарные операции нужно синхронизировать
        }
    }
}
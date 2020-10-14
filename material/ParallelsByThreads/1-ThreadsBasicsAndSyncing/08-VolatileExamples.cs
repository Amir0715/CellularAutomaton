using System.Threading;

namespace ParallelsByThreads
{
    public class VolatileExamples
    {
        // Возьмём обычное числовое поле
        private static int field = 0;
        
        // И функцию, которая очень активно использует значение этого поля
        private static void Awaiter()
        {
            field = 1;
            while (field == 1) { }
        }
        
        // Рассмотрим, как компилятор будет нам "помогать". Чтоб он нам "помог", надо обязательно
        // запускать этот пример в Relese режиме сборки, т.е. с оптимизациями. В режиме Debug
        // зависания не будет
        public static void CachedVariableValue()
        {
            // Возьмём поток
            var thread = new Thread(Awaiter);
            thread.Start();
            // Подождём немного
            Thread.Sleep(1000);
            // В этот момент поток должен завершиться, ведь field = 0 != 1
            field = 0;
            thread.Join();
            // Но нет, программа зависнет. Значение field закэшировалось для другого потока,
            // поэтому её изменение в этом потоке не повлияет на другой. Увы
        }
        
        // Теперь пометим поле как volatile
        private static volatile int fieldVolatile = 0;
        
        // Код этой функции точно такой же
        private static void AwaiterVolatile()
        {
            fieldVolatile = 1;
            while (fieldVolatile == 1) { }
        }

        public static void VolatileVariableValueDoNotCached()
        {
            var thread = new Thread(AwaiterVolatile);
            thread.Start();
            Thread.Sleep(1000);
            fieldVolatile = 0;
            thread.Join();
            // Всё корректно завершается. Модификатор volatile указывает компилятору, что
            // не надо делать оптимизаций со значением поля. Так что при каждом обращении
            // к fieldVolatile идёт обращение не в кэш потока, а к памяти, где реально лежит
            // значение переменной
            // Подробнее: https://habr.com/ru/post/130318/
        }
    }
}
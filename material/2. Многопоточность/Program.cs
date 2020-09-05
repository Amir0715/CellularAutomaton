using Parprog.Threading._2._Примитивы_синхронизации;

namespace Parprog.Threading
{
    class Program
    {
        private static void Main(string[] args)
        {
            BasicThreading.ExampleCurrentThreadInfo();
            BasicThreading.ExampleCreateAndStartThread();
            BasicThreading.ExampleParametrizedThread();
            BasicThreading.ExampleRaceCondition();
            
            SyncPrimitives.ExampleLocker();
            SyncPrimitives.ExampleMutex();
            SyncPrimitives.ExampleSemaphore();
        }
    }
}
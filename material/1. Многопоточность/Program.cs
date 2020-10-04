namespace Parprog.Threading
{
    class Program
    {
        private static void Main(string[] args)
        {
            Threads.ExampleCurrentThreadInfo();
            Threads.ExampleCreateAndStartThread();
            Threads.ExampleParametrizedThread();
            
            SyncErrors.ExampleRaceCondition();
            
            SyncPrimitives.ExampleLocker();
            SyncPrimitives.ExampleMutex();
            SyncPrimitives.ExampleSemaphore();
        }
    }
}
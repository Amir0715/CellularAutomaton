using System.Threading;

namespace ParallelsByThreads
{
    class Program
    {
        private static void Main(string[] args)
        {
            ResultsContainer.Attach("--- EXAMPLES ---");
            
            // ThisThreadExamples.ViewCurrentThreadStats();
            
            // ThreadBasicOperationsExamples.CreateAndStartThread();
            // ThreadBasicOperationsExamples.CreateAndStartParametrizedThread();
            
            // RaceConditionExamples.ChangeAfterCondition();
            // RaceConditionExamples.NonAtomicOperations();
            
            // LockExamples.ChangeAfterCondition();
            // LockExamples.NonAtomicOperations();
            
            // MutexExamples.ChangeAfterCondition();
            // MutexExamples.ReallyUsefulMutex();
            
            // DeadlockExample.DeadlockByLockers();
            // DeadlockExample.DeadlockByJoin();
            
            // VolatileExamples.CachedVariableValue();
            // VolatileExamples.VolatileVariableValueDoNotCached();
            
            // AtomicOperationsExamples.AtomicAndNoneVariants();

            // --- --- --- --- ---
            
            // ConcurrentQueueExamples.CommonQueueInMultithreading();
            // ConcurrentQueueExamples.ConcurrentQueueInMultithreading();
            
            // ConcurrentDictionaryExamples.ConcurrentDictionaryAsCache();
            
            // NativeThreadPoolExamples.BasicThreadPoolUsage();
            
            PrincipalThreadPoolExamples.HowToUse();
            
            ResultsContainer.Attach("--- EXAMPLES ---");
            ResultsContainer.Print();
        }
    }
}
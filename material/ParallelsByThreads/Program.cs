using System.Threading.Tasks;

namespace ParallelsByThreads
{
    class Program
    {
        private static async Task Main(string[] args)
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
            
            // PrincipalThreadPoolExamples.HowToUse();
            
            // await TaskExamples.AsyncOperations(); // Попробуйте убрать здесь await и убрать async из Main
            // await Task.Delay(1000);
            // await TaskExamples.ReadWriteFileAsync();
            // ResultsContainer.Attach("--- AND ---");
            // TaskExamples.ReadWriteFileNotAsync();

            // await MoreTaskExamples.HowToHandleExceptions();
            // await MoreTaskExamples.HowToWaitForTasks();

            // await ChannelsExamples.BasicUsage();

            await AsyncEnumerableExamples.BasicUsage();
            
            // await UsePowerOfAsyncAwait.Usage();
            
            ResultsContainer.Attach("--- EXAMPLES ---");
            ResultsContainer.Print();
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.core
{
    // TODO : statistics
    public class TaskManager
    {
        private Queue<Task> Tasks;
        
        public TaskManager()
        {
            Tasks = new Queue<Task>();
        }

        public void AddTask(Task task)
        {
            if (task is null)
                throw new TaskIsNullException("Task is null");
            else
                Tasks.Enqueue(task);
        }

        public void RunAll()
        {
            
            foreach (var t in Tasks)
            {
                t.Start();
            }
        }

        public void WaitAll()
        {
            if (Tasks.Count == 0)
            {
                throw new TasksSizeIsNullException("Size of task eq 0");
            }
            
            Task.WaitAll(Tasks.ToArray());
            
        }

        public void Clear()
        {
            Tasks.Clear();
        }
    }
}

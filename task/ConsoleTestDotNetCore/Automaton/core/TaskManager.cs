using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.core
{
    // TODO : statistics
    class TaskManager
    {
        private Queue<Task> tasks;
        

        public TaskManager()
        {
            tasks = new Queue<Task>();
        }

        public void AddTask(Task task)
        {
            if (task is null)
                throw new TaskIsNullException("Task is null");
            else
                tasks.Enqueue(task);
        }

        public void RunAll()
        {
            
            foreach (var t in tasks)
            {
                t.Start();
            }
        }

        public void WaitAll()
        {
            if (tasks.Count == 0)
            {
                throw new TasksSizeIsNullException("Size of task eq 0");
            }
            
            Task.WaitAll(tasks.ToArray());
            
        }

        private void Clear()
        {
            if (tasks.Count == 0)
            {
                throw new TasksSizeIsNullException("Size of task eq 0");
            }
            for (var i = 0; i < tasks.Count; i++)
            {
                tasks.Dequeue();
            }
            
        }
    }
}

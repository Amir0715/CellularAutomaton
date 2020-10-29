using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.core
{
    class TaskManager
    {
        private Queue<Task> tasks;
        private bool isStarted = false;

        public TaskManager()
        {
            tasks = new Queue<Task>();
        }

        public void AddTask(Task task)
        {
            if (task is null)
            {
                throw new TaskIsNullException("Task is null");
            }
            if (tasks.Count == 0)
            {
                tasks.Enqueue(task);
            }
        }

        public void RunAll()
        {
            if (!isStarted) return;
            isStarted = true;
            foreach (var t in tasks)
            {
                t.Start();
            }
        }

        public void WaitAll()
        {
            foreach (var task in tasks)
            {
                task.Wait();
            }
            isStarted = false;
        }

        public void Clear()
        {
            if (tasks.Count == 0)
            {
                throw new TasksSizeIsNullException("Size of task eq 0");
            }
            for (var i = 0; i < tasks.Count; i++)
            {
                tasks.Dequeue();
            }
            isStarted = false;
        }
    }
}

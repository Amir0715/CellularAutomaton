using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cellular_automaton.source
{
    class TaskManager
    {
        private Task[] tasks;
        private int size;
        public TaskManager()
        {
        }

        public void AddTask(Task task)
        {
            if (task is null)
            {
                //FIXME:
            }
            if(size == 0)
            {
                tasks = new Task[size+1];
            }
            else
            {
                System.Array.Resize(ref tasks, size+1);
            }
            tasks[size] = task;
            //return tasks[size++];
            size++;
        }

        public void RunAll()
        {
            foreach(var t in tasks)
            {
                
                t.Start();
            }
        }

        public void WaitAll()
        {
            Task.WaitAll(tasks);
        }
    }
}

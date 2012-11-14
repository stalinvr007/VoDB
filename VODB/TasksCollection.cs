using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace VODB
{
    internal sealed class TasksCollection
    {
        private readonly ICollection<Task> _tasks = new List<Task>();

        public TasksCollection(params Task[] tasks)
        {
            _tasks = tasks.ToList();
        }

        public void Add(Task task)
        {
            lock (_tasks)
            {
                _tasks.Add(task);    
            }
        }

        public int Count {
            get
            {
                lock (_tasks)
                {
                    foreach (var task in _tasks.Where(task => task.IsCompleted).ToList())
                    {
                        _tasks.Remove(task);
                    }
                    return _tasks.Count;    
                }
            }
        }

        public void StartAll()
        {
            lock (_tasks)
            {
                foreach (var thread in _tasks)
                {
                    thread.Start();
                }    
            }
        }

        public void JoinAll()
        {
            lock (_tasks)
            {
                foreach (var thread in _tasks)
                {
                    thread.Wait();
                }    
            }
        }
    }
}
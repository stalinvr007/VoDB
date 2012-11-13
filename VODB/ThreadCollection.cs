using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VODB
{
    internal sealed class ThreadCollection
    {
        readonly IEnumerable<Task> _threads = new List<Task>();

        public ThreadCollection(params Task[] tasks)
        {
            _threads = tasks;
        }
        
        public void StartAll()
        {
            foreach (var thread in _threads)
            {
                thread.Start();
            }
        }

        public void JoinAll()
        {
            foreach (var thread in _threads)
            {
                thread.Wait();
            }
        }

    }
}

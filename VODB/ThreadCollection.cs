﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace VODB
{
    internal sealed class ThreadCollection
    {
        readonly IEnumerable<Thread> _threads = new List<Thread>();

        public ThreadCollection(params Thread[] threads)
        {
            _threads = threads;
        }

        public ThreadCollection(params Action[] actions)
        {
            _threads = actions.Select(a => new Thread(() => a()));
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
                if (thread.IsAlive)
                {
                    thread.Join();
                }
            }
        }

    }
}

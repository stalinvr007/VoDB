using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VODB.Tests
{
    static class WatchExtensions
    {

        public static Stopwatch Run( this Stopwatch watch, Action action)
        {
            watch.Start();
            try
            {
                action();
            }
            finally
            {
                watch.Stop();
            }
            return watch;
        }

    }
}

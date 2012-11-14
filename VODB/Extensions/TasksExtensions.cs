using System.Threading.Tasks;

namespace VODB.Extensions
{
    internal static class TasksExtensions
    {

        /// <summary>
        /// Starts the task and returns it.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public static Task<TResult> RunAsync<TResult>(this Task<TResult> task)
        {
            task.Start();
            return task;
        }

    }
}

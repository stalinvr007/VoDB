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


        /// <summary>
        /// Adds the task to the specified collection.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public static Task<TResult> Add<TResult>(this TasksCollection collection, Task<TResult> task )
        {
            collection.Add(task);
            return task;
        }

    }
}

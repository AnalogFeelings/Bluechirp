using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;

namespace Bluechirp.Library.Services.Environment
{
    /// <summary>
    /// Defines a service interface for action enqueuing.
    /// </summary>
    public interface IDispatcherService
    {
        /// <summary>
        /// Enqueues an <see cref="Action"/> with a priority level.
        /// </summary>
        /// <param name="function">The action to enqueue.</param>
        /// <param name="priority">The action's priority in the queue.</param>
        public Task EnqueueAsync(Action function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal);

        /// <summary>
        /// Enqueues a <see cref="Task"/> with a priority level.
        /// </summary>
        /// <param name="function">The task to enqueue.</param>
        /// <param name="priority">The task's priority in the queue.</param>
        /// <returns>A task that acts as a proxy for the one retured by the function.</returns>
        public Task EnqueueAsync(Func<Task> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal);

        /// <summary>
        /// Enqueues a function of return type <typeparamref name="T"/> with a priority.
        /// </summary>
        /// <typeparam name="T">The return type of the function.</typeparam>
        /// <param name="function">The function to enqueue.</param>
        /// <param name="priority">The function's priority in the queue.</param>
        /// <returns>The return value of the function.</returns>
        public Task<T> EnqueueAsync<T>(Func<T> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal);

        /// <summary>
        /// Enqueues a <see cref="Task{TResult}"/> with a priority.
        /// </summary>
        /// <typeparam name="T">The return type of the task.</typeparam>
        /// <param name="function">The task to enqueue.</param>
        /// <param name="priority">The task's priority in the queue.</param>
        /// <returns>A task that acts as a proxy for the one returned by the function.</returns>
        public Task<T> EnqueueAsync<T>(Func<Task<T>> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal);
    }
}

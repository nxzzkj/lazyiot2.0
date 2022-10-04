using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 定义一个任务管理类，改类实现了任务并发处理，从而减少cpu
/// </summary>
namespace Scada.DBUtility
{
    public class LimitedTaskScheduler : TaskScheduler
    {
        public Action<string> TaskRunException;
        public Action<string> TaskCanceled;
        public Action<string> TaskRanToCompletion;
        /// <summary>Whether the current thread is processing work items.</summary> 
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;
        /// <summary>The list of tasks to be executed.</summary> 
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 
        /// <summary>The maximum concurrency level allowed by this scheduler.</summary> 
        private readonly int _maxDegreeOfParallelism;
        /// <summary>Whether the scheduler is currently processing work items.</summary> 
        private int _delegatesQueuedOrRunning = 0; // protected by lock(_tasks) 

        /// <summary> 
        /// Initializes an instance of the LimitedConcurrencyLevelTaskScheduler class with the 
        /// specified degree of parallelism. 
        /// </summary> 
        /// <param name="maxDegreeOfParallelism">The maximum degree of parallelism provided by this scheduler.</param> 
        public LimitedTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        /// <summary>
        /// current executing number;
        /// </summary>
        public int CurrentCount { get; set; }

        /// <summary>Queues a task to the scheduler.</summary> 
        /// <param name="task">The task to be queued.</param> 
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the list of tasks to be processed. If there aren't enough 
            // delegates currently queued or running to process tasks, schedule another. 
            lock (_tasks)
            {
                Console.WriteLine("Task Count : {0} ", _tasks.Count);
                _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    ++_delegatesQueuedOrRunning;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }
        int executingCount = 0;
        private static object executeLock = new object();
        /// <summary> 
        /// Informs the ThreadPool that there's work to be executed for this scheduler. 
        /// </summary> 
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                // Note that the current thread is now processing work items. 
                // This is necessary to enable inlining of tasks into this thread. 
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue. 
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // When there are no more items to be processed, 
                            // note that we're done processing, and get out. 
                            if (_tasks.Count == 0)
                            {
                                --_delegatesQueuedOrRunning;

                                break;
                            }

                            // Get the next item from the queue 
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }

                        TaskMonitor(item);
                        // Execute the task we pulled out of the queue 
                        base.TryExecuteTask(item);
                    }
                }
                // We're done processing items on the current thread 
                finally { _currentThreadIsProcessingItems = false; }
            }, null);
        }

        /// <summary>Attempts to execute the specified task on the current thread.</summary> 
        /// <param name="task">The task to be executed.</param> 
        /// <param name="taskWasPreviouslyQueued"></param> 
        /// <returns>Whether the task could be executed on the current thread.</returns> 
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {

            // If this thread isn't already processing a task, we don't support inlining 
            if (!_currentThreadIsProcessingItems) return false;

            // If the task was previously queued, remove it from the queue 
            if (taskWasPreviouslyQueued) TryDequeue(task);

            // Try to run the task. 
            return base.TryExecuteTask(task);
        }
        private void TaskMonitor(Task _task)
        {
            ///如果任务异常则，在此处输出异常
            _task.ContinueWith(r =>
            {

                if (TaskRunException != null && _task.Exception != null)
                {
                    string Exception = Convert.ToString(_task.Exception);
                    TaskRunException(Exception);
                }
                
            }, TaskContinuationOptions.OnlyOnFaulted);
            //任务取消的时候返回信息
            _task.ContinueWith(r =>
            {

                if (TaskCanceled != null)
                {

                    TaskCanceled(r.Id.ToString());
                }
               

            }, TaskContinuationOptions.OnlyOnCanceled);
            ///任务执行完成的时候返回的信息
            _task.ContinueWith(r =>
            {

                if (TaskRanToCompletion != null)
                {
                    TaskRanToCompletion(r.Id.ToString());

                }
             

            }, TaskContinuationOptions.OnlyOnRanToCompletion);

        }
        /// <summary>Attempts to remove a previously scheduled task from the scheduler.</summary> 
        /// <param name="task">The task to be removed.</param> 
        /// <returns>Whether the task could be found and removed.</returns> 
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks) return _tasks.Remove(task);
        }
        public      void  Add(Task task)
        {
            lock (_tasks)   _tasks.AddLast(task);
            
        }
        /// <summary>Gets the maximum concurrency level supported by this scheduler.</summary> 
        public sealed override int MaximumConcurrencyLevel { get { return _maxDegreeOfParallelism; } }

        /// <summary>Gets an enumerable of the tasks currently scheduled on this scheduler.</summary> 
        /// <returns>An enumerable of the tasks currently scheduled.</returns> 
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (lockTaken) return _tasks.ToArray();
                else throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken) Monitor.Exit(_tasks);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfPlayground.Multithreading
{
    /* Todo
     */

    public class Tasks01
    {
        public Tasks01()
        {
            T01();            
        }

        public void T01()
        {
            CancellationTokenSource cs = new CancellationTokenSource();

            Task.Run(() => {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(1000);
                    if (cs.Token.IsCancellationRequested)
                    {
                        Console.WriteLine("CXL'ed");
                        return;
                    }

                    //cs.Token.ThrowIfCancellationRequested();  //Another option for cancel
                }                

            }, cs.Token);

            Thread.Sleep(3000); //Let bg thread run a little
            cs.Cancel();

            Thread.Sleep(12000);    //We dont want app to end abruptly
        }

        //Hints to Task scheduler
        public void T02()
        {
            Task.Factory.StartNew(() =>
            {
            }
            , TaskCreationOptions.PreferFairness);

            //Hints to TaskScheduler
            //TaskCreationOptions.PreferFairness    you want task to run sooner than later
            //TaskCreationOptions.LongRunning       should more aggressively create thread pool threads
            //TaskCreationOptions.AttachedToParent  Associates a task with its parent task
            
            //Outside task wont complete until inside task completes bc of attachtoparent
            Task.Factory.StartNew(() =>
            {
                Task.Factory.StartNew(() => { }, TaskCreationOptions.AttachedToParent);
            });
        }

        //Continuations
        public void T03()
        {
            //When you spawn a bg thread and then have calling thread wait(), it gets blocked.
            //There is a better way to find out when your task has finished

            TaskScheduler ts = TaskScheduler.Default;   //This schedules onto the threadpool 
            TaskScheduler tsui = TaskScheduler.FromCurrentSynchronizationContext();   //This schedules onto the ui

            //This uses default task scheduler and runs on thread pool thread
            Task<int> t = Task.Run<int>(() => { return 5; });

            //Uses the synchronization context task scheduler and runs on GUI thread
            //You can pass different continuation options
            t.ContinueWith((ta) => { Console.WriteLine(ta.Result); }
                , CancellationToken.None
                , TaskContinuationOptions.ExecuteSynchronously
                , tsui
                );


            t.ContinueWith((ta) => { Console.WriteLine(ta.Result); }
                , CancellationToken.None
                , TaskContinuationOptions.OnlyOnRanToCompletion
                , tsui
                );

            t.ContinueWith((ta) => { Console.WriteLine(ta.Result); }
                , CancellationToken.None
                , TaskContinuationOptions.OnlyOnCanceled
                , tsui
                );

            t.ContinueWith((ta) => { Console.WriteLine(ta.Result); }
                , CancellationToken.None
                , TaskContinuationOptions.OnlyOnFaulted
                , tsui
                );            
        }

        //You might want to create tasks with similar configuration
        public void T04()
        {
            Task parent = new Task(() => {
                var cts = new CancellationTokenSource();
                TaskFactory<int> tf = new TaskFactory<int>(
                                            cts.Token
                                            , TaskCreationOptions.AttachedToParent
                                            , TaskContinuationOptions.ExecuteSynchronously
                                            , TaskScheduler.Default);

                Task<int>[] childtasks = new Task<int>[] {
                    tf.StartNew(()=> { return 5; })
                    ,tf.StartNew(()=> { return 6; })
                    ,tf.StartNew(()=> { return 7; })
                    
                };

                tf.ContinueWhenAll(childtasks, ct => ct.Where(t => t.Status == TaskStatus.RanToCompletion).Max(t => t.Result))
                .ContinueWith(t => Console.WriteLine("Max val is " + t.Result), TaskContinuationOptions.ExecuteSynchronously);
            });
        }

        /* **************************Task-based Asynchronous Pattern**************************
         * The main method is TapMethod01()
         * You may have somone provide you with an api that has an async method like ConnectToService
         * This ConnectToService will raise an event when you are connected.  To be notified when you
         * are connected, you register a callback or subscribe to an event
         * Instead of writing your code in the callback, we can wrap this code in a Task, use TaskCompletionSource,
         * have the callback or event handler return the TaskCompletionSource, and await the Task
         */
        public async void TapMethod01()
        {
            Task<string> t = this.ButtonClicked();
            //any code here
            var res = await t;
        }

        TaskCompletionSource<string> ButtonClickCompleted = new TaskCompletionSource<string>();
        private Task<string> ButtonClicked()
        {
            var b = new System.Windows.Controls.Button();
            b.Click += Tasks01_Click;
            b.RaiseEvent(new System.Windows.RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
            return ButtonClickCompleted.Task;
        }

        private void Tasks01_Click(object sender, System.Windows.RoutedEventArgs e) => ButtonClickCompleted.SetResult("Done");

        /* **************************Task-based Asynchronous Pattern**************************/
    }

    /// <summary>
    /// This class will show you where exception occur and how its delivered in a multithreaded environment
    /// </summary>
    public class ThreadsAndExceptions
    {
        public void T01()
        {
            try
            {
                new Thread(go01B).Start();
            }
            catch (Exception ex)
            {
                //Never gets reached
                Console.WriteLine("ExceptionMain");
            }
        }

        public void go01A()
        {
            throw null;
        }

        public void go01B()
        {
            try
            {
                throw null;
            }
            catch (Exception ex)
            {
                //Gets Reached
                Console.WriteLine("ExceptionThread");
            }
        }
    }

    /* http://www.albahari.com/threading/part5.aspx#_Parallel_Programming
     * https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/data-parallelism-task-parallel-library
     */

    public class ParallelStuff
    {
        /* Parallel invoke, for, foreach will all run your methods in parallel
           Each item may not be a Task, not 1 to 1 as there may be optimizations
           Parallel will block so to do this on a UI thread, wrap it in Task.Run
           You can always await Task.whenAll and pass an array of tasks
           Parallel was meant to block and be efficient by taking a collection, partitioning it,
           and spreading the work onto multiple threads
        */

        //Executes an array of delegates in parallel.  This is differenty than a collection of tasks bc this works more efficiently
        //if you pass in many delegates because it partitions it into batches which it assigns to a handful of underlying Tasks 
        //If you use tasks, you would assign a task to each delegate
        //Parallels invoke, for, and foreach block until work is done.  To not block, put the parallel inside a Task.Run()
        public void T10()
        {
            Parallel.Invoke(
                 () => new WebClient().DownloadFile("http://www.linqpad.net", "lp.html"),
                 () => new WebClient().DownloadFile("http://www.jaoo.dk", "jaoo.html"));
        }

        public void T11()
        {
            List<int> PFE = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Parallel.For(0, 20, i => Console.WriteLine(i));
        }

        public void T12()
        {
            List<int> PFE = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Parallel.ForEach(PFE, x => { Console.WriteLine(x); });
        }

        public void T13()
        {
            //This makes sure when Parallelizing you don't block the UI thread
            Task.Run(T12);
        }
    }
}

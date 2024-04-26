using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfPlayground.Multithreading
{
    /*     
     * Blocking vs spinning
     * Reading a collection vs writing a collection, reader/writer locks
     * Thread affinity
     * Synchronization context
     
     * Atomicity
     * If a group of variables are always read and written within the same lock, you can say the variables 
     * are read and written atomically
     * Instruction atomicity is a different, although analogous concept: an instruction is atomic if it 
     * executes indivisibly on the underlying processor 
     * A statement is intrinsically atomic if it executes as a single indivisible instruction on the 
     * underlying processor. Strict atomicity precludes any possibility of preemption. A simple read or 
     * write on a field of 32 bits or less is always atomic. Operations on 64-bit fields are guaranteed 
     * to be atomic only in a 64-bit runtime environment, and statements that combine more than one 
     * read/write operation are never atomic:
     
     * Nonblocking Synchronization
     * Although locking can always satisfy synchronization, a contended lock means that a thread must block, 
     * suffering the overhead of a context switch and the latency of being descheduled, 
     * which can be undesirable in highly concurrent and performance-critical scenarios
     * Non blocking is the answer
     * Use Interlocked 
     * Interlocked can never suffer the additional cost of context switching due to blocking but using Interlocked 
     * within a loop with many iterations can be less efficient than obtaining a single lock around the 
     * loop (although Interlocked enables greater concurrency).

     * Spinning (Make an example)
     * A brief episode of spinning is often preferable to blocking, as it avoids the cost of context switching 
     * and kernel transitions
     * 
     * 
     * WCF
     * Create Service
     *  New proj: IService: ServiceContrace, Service: Implementation, App.config:Endpoint ABC
     * Client
     *  Add service reference, nuget pkg
     *  Client configuration file, defines the endpoint to connect to the service
    
     */


    public class ThreadSynchronization
    {
        /* These are synchronization objects
         * When you lock a piece of code, multiple threads will wait until the lock is released
         * If you use the lock in multiple places like in a collection for Add and remove, they still
         * wait until lock is removed
         * A field dedicated for the purpose of locking (such as _locker) allows precise control over the scope and granularity of the lock
         */
        private readonly object _locker = new object();

        private readonly object _locker01 = new object();
        private readonly object _locker02 = new object();

        public ThreadSynchronization()
        {
            T01();
        }

        /* Race Condition
         * 2 or more threads reach a certain block of code and the outcome is unpredictable
         * Running the code multiple times poduces different results 
         * The result on any run cannot be predicted
         */

        //We have 2 threads taking in the value and iteration
        //The first loop will take 0, 100 million and final value = 100million
        //The second loop will now have a starting value of 100million so
        //100million, 100million and final value = 200million
        //If you remove the lock, final value will be off since both threads race for the variable value.  
        //By the time the second thread starts, the first thread has already incremented the value
        //The lock will make the second  thread block until the first thread is done and releases the lock
        //When you have many threads trying to access the same piece of code that is locked (critical section),
        //we call it contention
        public void T01()   
        {
            int iterations = 100000000; //100,000,000
            int numTasks = 2;
            List<Task> Tasks = new List<Task>();
            int value = 0;
            
            for (int i = 0; i < numTasks; i++)
            {
                Task t = Task.Run(() =>
                {
                    IncrementValue(ref value, iterations);
                });

                Tasks.Add(t);
            }

            Task.WaitAll(Tasks.ToArray());
            //Console.WriteLine("Expected: {0}, Actual: {1}", numTasks * iterations, value);
            Trace.WriteLine(string.Format("Expected: {0}, Actual: {1}", numTasks * iterations, value));
        }

        public void IncrementValue(ref int value, int iterations)
        {            
            //This is atomic, if you remove the lock, the value will be off
            lock (_locker)
            {
                for (int i = 0; i < iterations; i++)
                {
                    value = value + 1;
                }
            }
        }

        //Using locks to make sure values are updated properly.  You use a lock and lock around a common locking object
        //Here, you dont want networth to give you a wrong value when someone puts cash into your account
        public void T02()
        {
            decimal cash=50;
            decimal receivables=50;
            decimal networth = 0;

            Action<decimal> ReceivePayment = (amount) => 
            {
                lock(_locker)
                {
                    cash += amount;
                    receivables -= amount;
                }
            };

            Action NetWorth = () => 
            {
                lock (_locker)
                {
                    networth = cash + receivables;                    
                }
            };

            Task.Run(() => { ReceivePayment(10); });

            Task.Run(NetWorth);

            Thread.Sleep(100);  //without this, writline will run before Task NetWorth

            Trace.WriteLine(networth);
        }

        //When to lock
        //you need to lock around accessing any writable shared field. 
        //Even in the simplest case — an assignment operation on a single field — you must consider synchronization.
        public void T03()
        {
            //In the following class, neither the Increment nor the Assign method is thread-safe:

            //static int _x;
            //static void Increment() { _x++; }
            //static void Assign()    { _x = 123; }

            //This is thread safe
            //static readonly object _locker = new object();
            //static int _x;
            //
            //static void Increment() { lock (_locker) _x++; }
            //static void Assign()    { lock (_locker) _x = 123; }
        }

        //The lock statement is a syntactic shortcut for a call to the 
        //methods Monitor.Enter and Monitor.Exit, with a try/finally block.        
        public void T11()
        {
            int _val1 = 1, _val2 = 2;

            Monitor.Enter(_locker);
            try
            {
                if (_val2 != 0) Trace.WriteLine(_val1 / _val2);
                _val2 = 0;
            }
            finally { Monitor.Exit(_locker); }
        }

        public void InterLockExamples()
        {
            int myField = 0;                        
            lock (_locker) { myField++; } //instead of this
            Interlocked.Increment(ref myField); //use this

            object x = 1, y = 2;
            if (x == null) lock (_locker) x ??= y; //instead of this
            Interlocked.CompareExchange(ref x, y, null); //use this
        }

        //Semaphore
        public void T04()
        {
            SemaphoreSlim ss = new SemaphoreSlim(2);
            
            Action<int> a1 = (i) => {

                Trace.WriteLine(i + " wants to enter");
                ss.Wait();
                Trace.WriteLine(i + " is inside Semaphore");
                Thread.Sleep(2000);
                ss.Release();
                Trace.WriteLine(i + " is out");
            };

            for (int j = 0; j < 5; j++)
            {
                int k = j;
                Task.Run(() => { a1(k); });
            }
        }

        //Signaling

        //AutoResetEvent
        //A thread that calls Wait one pauses until value is set, then continues and then closes automatically
        //Many threads may be blocked at the wait one so if an unblocked thread calls set once, only one blocked 
        //waiting thread will pass through
        public void T05()
        {
            EventWaitHandle autoreset = new AutoResetEvent(false);

            Action<int> a1 = (i) => {

                Trace.WriteLine(i + " waiting for signal");
                autoreset.WaitOne();
                Trace.WriteLine("Signal received, " + i + " continues");
            };

            Task.Run(() => { a1(1); });
            Task.Run(() => { a1(2); });
            Task.Run(() => { a1(3); });

            Thread.Sleep(1000);
            autoreset.Set();

            Thread.Sleep(1000);
            autoreset.Set();

            /*don't do below because calling it 2 times doesn't mean it will allow 2 threads.  It just sets it open
            
            autoreset.Set();
            autoreset.Set();
            
            */
        }

        //ManualResetEvent
        //Works like AutoResetEvent but if someone calls set, it allows all blocked waiting threads to pass.
        //It closes once someone calls reset.
        public void T06()
        {
            EventWaitHandle autoreset = new ManualResetEvent(false);

            Action<int> a1 = (i) => {

                Trace.WriteLine(i + " waiting for signal");
                autoreset.WaitOne();
                Trace.WriteLine("Signal received, " + i + " continues");

            };

            Task.Run(() => { a1(1); });
            Task.Run(() => { a1(2); });
            Task.Run(() => { a1(3); });

            Thread.Sleep(1000);
            autoreset.Set();
        }

        //CountdownEvent
        //Works opposite of manualresetevent in that it waits until a certain amount of signals are sent
        public void T07()
        {
            CountdownEvent _countdown = new CountdownEvent(3);

            _countdown.Wait();   // Blocks until Signal has been called 3 times
            Console.WriteLine("All threads have finished speaking!");

            Action<object> SaySomething = (thing) =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(thing);
                _countdown.Signal();
            };

            new Thread(()=> { SaySomething("I am thread 1"); } ).Start();
            new Thread(() => { SaySomething("I am thread 2"); }).Start();
            new Thread(() => { SaySomething("I am thread 3"); }).Start();            
        }

        //Signaling with wait and pulse
        public void T08()
        {
        }

        //Volatile
        //The variable complete gets cached on 2nd thread so when its changed on main thread, 2nd thread doesn't see it
        //Make it volatile so process knows to check.  You can also use locks
        public void T09()
        {
            bool complete = false;
            var t = new Thread(() =>
            {
                bool toggle = false;
                while (!complete) toggle = !toggle;
            });
            t.Start();
            Thread.Sleep(1000);
            complete = true;
            t.Join();        // Blocks indefinitely
        }

        //Deadlock, Both threads wait for a resource held by the other, so neither can proceed.
        public void T10()
        {
            Task.Run(T10_01);
            Task.Run(T10_02);
        }

        public void T10_01()
        {
            Thread.Sleep(1500);
            Trace.WriteLine("Starting CS T02_01");
            lock (_locker01)
            {
                Thread.Sleep(1500);
                lock (_locker02)
                    Console.WriteLine("I'm inside CS T02_01");
                
            }
        }

        public void T10_02()
        {
            Thread.Sleep(1500);
            Trace.WriteLine("Starting CS T02_02");
            lock (_locker02)
            {
                Thread.Sleep(1500);
                lock (_locker01)
                    Trace.WriteLine("I'm inside CS T02_02");                
            }
        }

        //Another Deadlock, Both threads wait for a resource held by the other, so neither can proceed.
        public void T12()
        {
            object locker1 = new object();
            object locker2 = new object();

            new Thread(() =>
            {
                lock (locker1)
                {
                    Thread.Sleep(1000);
                    lock (locker2) ;      // Deadlock
                }
            }).Start();

            lock (locker2)
            {
                Thread.Sleep(1000);
                lock (locker1) ;                          // Deadlock
            }
        }

        //Mutex
        public void T13()
        {
            // Naming a Mutex makes it available computer-wide. Use a name that's
            // unique to your company and application (e.g., include your URL).

            /*
            using (var mutex = new Mutex(false, "oreilly.com OneAtATimeDemo"))
            {

                //Wait a few seconds if contended, in case another instance
                //of the program is still in the process of shutting down.

                if (!mutex.WaitOne(TimeSpan.FromSeconds(3), false))
                {
                    Console.WriteLine("Another app instance is running. Bye!");
                    return;
                }
                RunProgram();
            }
            */
        }
    }
}

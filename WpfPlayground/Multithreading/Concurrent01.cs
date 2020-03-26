using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPlayground.Multithreading
{
    public class ConcurrentT01
    {
        /*
         
         Why use concurrent collections over lock?
         Locks creates blocking. 
         Locks are one way to create thread safety.
         Other thread safety mechanisms
         Memory barriers control precisely what order variables are accessed in
         Special atomic assembly instructions that manipulate memory locations atomically.  The atomicity is
         enforced by the hardware
         Other synchronization primitives:AutoReset Event, Mutex, Reader Writer Lock, Semaphore
         You can also avoid the primitives with clever algorithms

            Doesnt protect from race condition
            RC ex. 
            Thread 1: read val, increment val, write val
            Thread 2: read val, increment val, write val
            if both threads increment the val, then when thread 1 writes the val, it will be incorrect
            Although this is thread safe, the data is wrong

            You can use a concurrentDictionary<int, object> to replace List<object> but list is much faster if looking up by index with lock
            Although dictionary key is hash, you have to calculate the hash of the key, then work out which bucket that should be in, 
            possibly deal with duplicate hashes (or duplicate buckets) and then check for equality
                                   

            Volatile does not guarantee atomicity.  Volatility just means you get a fresh variable

        */

        public ConcurrentT01()
        {
            CT03();
        }

        private void CT01()
        {
            //Places orders and prints in order
            var orders = new Queue<string>();
            PlaceOrder(orders, "Alan");
            foreach (var order in orders) { Console.WriteLine(order); }
        }

        private void CT02()
        {
            //Places orders and prints incorrectly or crashes
            //unable to add items safely from different threads
            var orders = new Queue<string>();
            var task1 = Task.Run(() => PlaceOrder(orders, "Alan"));
            var task2 = Task.Run(() => PlaceOrder(orders, "Stb"));

            Task.WaitAll(task1, task2);

            foreach (var order in orders) { Console.WriteLine(order); }
        }

        private void CT03()
        {
            //Places orders from different threads safely
            var orders = new ConcurrentQueue<string>();
            var task1 = Task.Run(() => PlaceOrderConcurrent(orders, "Alan"));
            var task2 = Task.Run(() => PlaceOrderConcurrent(orders, "Stb"));

            Task.WaitAll(task1, task2);

            foreach (var order in orders) { Console.WriteLine(order); }
        }

        private void CT04()
        {
            //Places orders from different threads safely
            var orders = new ConcurrentQueue<string>();
            var task1 = Task.Run(() => PlaceOrderConcurrent(orders, "Alan"));
            var task2 = Task.Run(() => PlaceOrderConcurrent(orders, "Stb"));

            

            Task.WaitAll(task1, task2);

            foreach (var order in orders) { Console.WriteLine(order); }
        }

        static void PlaceOrder(Queue<string> orders, string name)
        {
            //Not thread safe
            for (int i = 0; i < 5; i++)
            {
                orders.Enqueue(string.Format("{0} placed order for item {1} ", name, i + 1));
            }
        }

        private static readonly object locker = new object();
        static void PlaceOrderSafe(Queue<string> orders, string name)
        {
            //Thread safe but must remember to lock any access to the queue
            for (int i = 0; i < 25; i++)
            {
                lock (locker)
                {
                    orders.Enqueue(string.Format("{0} placed order for item {1} ", name, i + 1));
                }
            }
        }

        static void PlaceOrderConcurrent(ConcurrentQueue<string> orders, string name)
        {
            //Thread safe
            
            for (int i = 0; i < 25; i++)
            {
                orders.Enqueue(string.Format("{0} placed order for item {1} ", name, i + 1));
            }            
        }        
    }
}

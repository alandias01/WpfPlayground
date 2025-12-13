using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace WpfPlayground.DesignPatterns.Behavioral
{
    /* Producer Consumer
     * CPU or IO bound refers to the processing function
     * 
     * Data Exist already (Fixed collection)
     *  CPU Bound
     *   Simple loop → Parallel.For / Parallel.ForEach
     *   LINQ query → PLINQ (AsParallel)
     *   Runtime auto-scales to Environment.ProcessorCount
     *   Best for crunching numbers, transformations, simulations
     *   Parallel APIs shine because they keep cores busy crunching.
     *   
     *  IO Bound
     *   Parallel.ForEach is not ideal (blocks threads while waiting)
     *   Parallel APIs waste threads (they block while waiting)
     *   
     *   Prefer async fan-out with Task.WhenAll or Parallel.ForEachAsync (.NET 6+)
     *   Async/await with Task.WhenAll is better, because the thread pool can reuse threads while awaiting I/O.
     *   Example: map collection to tasks → await Task.WhenAll(tasks)
     *   
     * Data is streamed
     *  IO Bound
     *   Channel<T> with async consumers
     *    Fan-out tasks per item (with concurrency cap)
     *    Await async operations inside consumer
     *  
     *  CPU Bound
     *   Channel<T> or BlockingCollection<T>
     *    Multiple consumers = Environment.ProcessorCount
     *    Bounded capacity for back-pressure
     * 
     */
    public class ProducerConsumer
    {
        public ProducerConsumer() 
        {
            FixedCollectionCPUBound();
        }

        private void BasicProducerConsumer()
        {
            var channel = Channel.CreateUnbounded<int>();
            Task p = Task.Run(() => Enumerable.Range(0, 10).ToList().ForEach(x => channel.Writer.TryWrite(x)));
            
        }

        private void FixedCollectionCPUBound()
        {
            var data = Enumerable.Range(1, 20);

            // Parallel.ForEach
            Parallel.ForEach(data, item =>
            {
                // CPU-heavy work
                double result = Math.Sqrt(item) * Math.PI;
                Console.WriteLine($"Processed {item} → {result}");
            });

            // PLINQ
            var results = data.AsParallel()
                              .Select(item => Math.Sqrt(item) * Math.PI)
                              .ToList();
        }

        private async Task FixedCollectionIOBound()
        {
            var urls = new[] { "https://example.com/a", "https://example.com/b" };

            var tasks = urls.Select(async url =>
            {
                using var client = new HttpClient();
                string content = await client.GetStringAsync(url);
                Console.WriteLine($"Fetched {url} length={content.Length}");
            });

            await Task.WhenAll(tasks);
        }


        private async Task StreamingDataCPUBound()
        {
            var channel = Channel.CreateBounded<int>(10);

            // Producer
            _ = Task.Run(async () =>
            {
                for (int i = 0; i < 20; i++)
                {
                    await channel.Writer.WriteAsync(i);
                    Console.WriteLine($"Produced {i}");
                }
                channel.Writer.Complete();
            });

            // Consumers
            int consumerCount = Environment.ProcessorCount;
            Task[] consumers = Enumerable.Range(0, consumerCount)
                .Select(_ => Task.Run(async () =>
                {
                    await foreach (var item in channel.Reader.ReadAllAsync())
                    {
                        // CPU-heavy work
                        double result = Math.Sqrt(item) * Math.PI;
                        Console.WriteLine($"Consumed {item} → {result}");
                    }
                }))
                .ToArray();

            await Task.WhenAll(consumers);
        }

        /* 
         * SemaphoreSlim(4) → limits concurrency to 4 tasks at a time.
         * WaitAsync() → consumer waits until a slot is available before starting a new task.
         * Release() → frees the slot when the task finishes, allowing another to start.
         * This prevents flooding the thread pool if the producer is fast or the workload is heavy.
         * You can tune the cap (e.g., Environment.ProcessorCount * 2) depending on whether work is CPU‑bound or I/O‑bound.
         * With this pattern, you get streaming semantics, parallelism, and safety. The channel keeps items flowing, tasks fan‑out 
         * for responsiveness, and the semaphore enforces a ceiling so you don’t overwhelm the system.
         */
        private async Task StreamingDataIOBound()
        {
            var channel = Channel.CreateUnbounded<string>();

            // Producer
            _ = Task.Run(async () =>
            {
                foreach (var url in new[] { "https://example.com/a", "https://example.com/b" })
                {
                    await channel.Writer.WriteAsync(url);
                    Console.WriteLine($"Produced {url}");
                }
                channel.Writer.Complete();
            });

            // Async consumers
            Task consumer = Task.Run(async () =>
            {
                await foreach (var url in channel.Reader.ReadAllAsync())
                {
                    _ = Task.Run(async () =>
                    {
                        using var client = new HttpClient();
                        string content = await client.GetStringAsync(url);
                        Console.WriteLine($"Fetched {url} length={content.Length}");
                    });
                }
            });

            await consumer;
        }

        static async Task StreamingDataIOBoundWithConcurrencyCap()
        {
            var channel = Channel.CreateUnbounded<string>();

            // Producer: stream URLs into channel
            _ = Task.Run(async () =>
            {
                foreach (var url in new[] { "https://example.com/a", "https://example.com/b", "https://example.com/c" })
                {
                    await channel.Writer.WriteAsync(url);
                    Console.WriteLine($"Produced {url}");
                    await Task.Delay(200); // simulate streaming arrival
                }
                channel.Writer.Complete();
            });

            // Concurrency cap: allow at most 4 concurrent fetches
            var semaphore = new SemaphoreSlim(4);

            // Consumer: fan-out tasks but respect concurrency cap
            Task consumer = Task.Run(async () =>
            {
                await foreach (var url in channel.Reader.ReadAllAsync())
                {
                    // Wait for a slot before starting new work
                    await semaphore.WaitAsync();

                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            using var client = new HttpClient();
                            string content = await client.GetStringAsync(url);
                            Console.WriteLine($"Fetched {url} length={content.Length}");
                        }
                        finally
                        {
                            // Release slot when finished
                            semaphore.Release();
                        }
                    });
                }
            });

            await consumer;
            Console.WriteLine("All items dispatched with concurrency cap.");
        }


        private void ProducerConsumer01()
        {
            var channel = Channel.CreateBounded<int>(new BoundedChannelOptions(5) { FullMode = BoundedChannelFullMode.Wait });

            Task p = Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    await channel.Writer.WriteAsync(i);
                    await Task.Delay(1000);
                }
                channel.Writer.Complete();
            });

            Task c = Task.Run(async () =>
            {
                await foreach (var item in channel.Reader.ReadAllAsync())
                {
                    Console.WriteLine($"Consuming {item}");
                    await Task.Delay(1000);
                }
            });

            Task.WhenAll(p, c);            
        }
    }
}

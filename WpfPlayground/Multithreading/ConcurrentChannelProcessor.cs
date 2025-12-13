using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace WpfPlayground.Multithreading
{
    /// <summary>
    /// Streaming support → Producer writes items as they arrive.
    /// Parallelism → Each item fans out into its own task.
    /// Concurrency cap → SemaphoreSlim ensures you never exceed maxConcurrency.
    /// Reusable → Just plug in your processing function and go.
    /// 
    /// this helper gives you a drop‑in component for any producer–consumer pipeline: 
    /// you decide the capacity, concurrency cap, and processing function, and it 
    /// handles the rest.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentChannelProcessor<T>
    {
        private readonly Channel<T> _channel;
        private readonly Func<T, Task> _processor;
        private readonly SemaphoreSlim _semaphore;

        public ConcurrentChannelProcessor(
            Func<T, Task> processor,
            int capacity = 100,
            int maxConcurrency = 4)
        {
            _channel = Channel.CreateBounded<T>(new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleWriter = false,
                SingleReader = true // one dispatcher, fan-out tasks
            });

            _processor = processor;
            _semaphore = new SemaphoreSlim(maxConcurrency);
        }

        public ChannelWriter<T> Writer => _channel.Writer;

        public async Task StartAsync()
        {
            await foreach (var item in _channel.Reader.ReadAllAsync())
            {
                await _semaphore.WaitAsync();

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _processor(item);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                });
            }
        }

        static async Task ConcurrentChannelProcessor_HowToUse()
        {
            // Define your processing function
            async Task ProcessItem(int item)
            {
                Console.WriteLine($"Processing {item} on Task {Task.CurrentId}");
                await Task.Delay(500); // simulate work
            }

            // Create processor with concurrency cap = 4
            var processor = new ConcurrentChannelProcessor<int>(ProcessItem, capacity: 10, maxConcurrency: 4);

            // Start consumer dispatcher
            var consumerTask = processor.StartAsync();

            // Producer: stream items
            for (int i = 0; i < 20; i++)
            {
                await processor.Writer.WriteAsync(i);
                Console.WriteLine($"Produced {i}");
            }

            processor.Writer.Complete();
            await consumerTask;

            Console.WriteLine("All items processed.");
        }
    }
}

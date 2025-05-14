using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace WpfPlayground.DesignPatterns.Behavioral
{
    /* Producer Consumer
     * A producer can be producig faster than a single producer
     * 
     * 
     * 
     * 
     */
    public class ProducerConsumer
    {
        public ProducerConsumer() => BasicProducerConsumer();

        private void BasicProducerConsumer()
        {
            var channel = Channel.CreateUnbounded<int>();
            Task p = Task.Run(() => Enumerable.Range(0, 10).ToList().ForEach(x => channel.Writer.TryWrite(x)));
            
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

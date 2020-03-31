using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfPlayground.Multithreading
{
    public class ProducerConsumer02
    {
        private int getDocumentLatencyMS = 10;
        private int processDocumentLatencyMS = 20;
                
        private ConcurrentQueue<int> documentSource;
        private ConcurrentDictionary<int, int> itemsDequeued = new ConcurrentDictionary<int, int>();

        public ProducerConsumer02()
        {
            this.documentSource = new ConcurrentQueue<int>(Enumerable.Range(0, 20));
            this.S1();
            //this.S2();
        }

        //20 items takes around 930ms        
        //GetDocumentIOBound is 10ms, ProcessDocument is 20ms
        //30ms *20 items = should take 600ms
        //930ms - 600ms = 330ms as overhead

        //This is doing things synchronously
        private async Task<long> S1()
        {
            var sw = Stopwatch.StartNew();

            foreach (var item in this.documentSource)
            {
                var result1 = await this.GetDocumentIOBound(item);
                var result2 = this.ProcessDocument(result1);
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        //20 items takes around 75ms        
        //GetDocumentIOBound is 10ms,ProcessDocument is 20ms
        //Tasks are added quickly
        //8 threads (since I have 8 cores).  Each thread is running the 20ms process
        // 20 items / 8 threads running 20ms each = 2.4 
        // Each round takes 20ms and can handle 8 items, 2 rounds is 40ms
        //We have to do a 3rd round but only need 4 threads so that another 20ms
        //75ms actual - 60ms = 15ms for overhead

        //This implementation is good when you have a predefined set of IO operations and small amount of IO tasks
        //If its not predefined and the items are coming in randomly, this will break
        //in the while loop since it will stop when count=0
        //Maybe have a mechanism to identify when done like volatile variable bool done?
        
        //Many IO Tasks
        //This is fine for a small amount of tasks but when you have 10k tasks,
        //we end up registering and unregistering upwards of 50 million continuations as part of the WhenAny calls
        //Look at S3()

        private async Task<double> S2()
        {
            var sw = Stopwatch.StartNew();

            var IOTasks = new List<Task<int>>();

            //Add tasks, happens quickly since we aren't waiting for tasks to complete
            foreach (var item in this.documentSource)
                IOTasks.Add(this.GetDocumentIOBound(item));
                        
            while (IOTasks.Count > 0)
            {                
                Task<int> firstFinishedTask = await Task.WhenAny(IOTasks);  //Gets the latest completed Task
                                
                IOTasks.Remove(firstFinishedTask);  //Remove so you dont reprocess
                                
                int result = await firstFinishedTask;   //Await the completed task to get result
                this.itemsDequeued.TryAdd(result, 0);   //Add result to a hash to see you didn't get dupes since hash doesn't allow dupes
            }

            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        //When we have many IO Tasks
        //https://devblogs.microsoft.com/pfxteam/processing-tasks-as-they-complete/
        private async Task<long> S3()
        {
            var sw = Stopwatch.StartNew();
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }
                
        private async Task<int> GetDocumentIOBound(int documentId)
        {
            await Task.Delay(this.getDocumentLatencyMS);
            return documentId;
        }
                
        public int ProcessDocument(int documentId)
        {
            Thread.Sleep(this.processDocumentLatencyMS);
            return documentId;
        }        
    }
}

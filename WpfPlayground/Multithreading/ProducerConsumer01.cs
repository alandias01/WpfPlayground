using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfPlayground.Multithreading
{
    /* 3 processes that have to be run in order on the data item
     * can we put the 3 processes in a task and partition the data, Data parallellism
     * Lets say each process takes 10ms but the 3rd process doesnt work well in parallel
     * then a thread would be blocked
     * If t1 and t2 access a process p3 that has a lock and t1 is inside that lock, then
     * t2 is blocked so it can run p3.  We can have t2 do other stuff like p1 and p2 on other 
     * data items
     * 
     * p1:10ms, p2:10ms, p3:10ms but 1 at a time
     * 
     * 1000ms/10ms = 100 so a process can process 100 items per second
     * 
     * We have 8 threads. 1 thread dedicated to p3, 7 threads for p1 and p2.
     * p1, p2 Time: 1000ms / (10ms +10ms) = 50 items per second * 7 threads = 350 items per second
     * p3 Time: 1000ms/10ms = 100 items per second
     * 
     * What this means is in 1 second 350 items will be put into a queue and 100 items will be processed by p3
     * So 250 items will accrue per second     * 
     */

    /* Results
     * 20 items
     * Scenario1:350
     * Scenario2:315
     * Scenario3:350
     * 
     * 50 items
     * Scenario1:680
     * Scenario2:650
     * Scenario3:715
     * 
     * 200 items
     * Scenario1:2320
     * Scenario2:2290
     * Scenario3:2315
     */

    public class ProducerConsumer01
    {
        private readonly object _locker = new object();
        private int readDocumentLatencyMS = 10;
        private int processDocumentLatencyMS = 10;
        private int SaveDocumentLatencyMS = 10;

        private BlockingCollection<int> bc = new BlockingCollection<int>();
        private ConcurrentQueue<int> documentSource;
        private ConcurrentDictionary<int, int> itemsDequeued = new ConcurrentDictionary<int, int>();

        public ProducerConsumer01()
        {
            this.documentSource = new ConcurrentQueue<int>(Enumerable.Range(0, 200));

            //var timeItTook = Scenario1();
            //var timeItTook = Scenario2();
            var timeItTook = Scenario3();
        }

        //Each step is done 1 after the other and data is partitioned without a queue
        private int Scenario1()
        {
            DateTime StartDateTime = DateTime.Now;

            Parallel.ForEach(documentSource, (docId) =>
            {
                this.itemsDequeued.TryAdd(docId, 0);
                var p1Result = this.ReadDocument(docId);
                var p2Result = this.ProcessDocument(p1Result);
                this.SaveDocument(p2Result);
            });

            DateTime EndDateTime = DateTime.Now;
            TimeSpan span = EndDateTime - StartDateTime;
            int ms = (int)span.TotalMilliseconds;
            return ms;
        }

        //7 threads for reading and processing.  1 thread for saving
        private int Scenario2()
        {
            DateTime StartDateTime = DateTime.Now;

            var producers = Enumerable.Range(0, 7).Select(_ =>
                Task.Run(() =>
                {
                    while(this.documentSource.Any())
                    {
                        int docId;
                        if (!this.documentSource.TryDequeue(out docId))                            
                            break;

                        itemsDequeued.TryAdd(docId, 0);
                        var p1Result = this.ReadDocument(docId);
                        var p2Result = this.ProcessDocument(p1Result);
                        bc.Add(p2Result);
                    }                    
                })
            );

            var consumer = Task.Run(() => 
            {
                foreach (var item in bc.GetConsumingEnumerable())
                {
                    this.SaveDocument(item);
                }
            });                

            Task.WaitAll(producers.ToArray());
            bc.CompleteAdding();
            consumer.Wait();

            DateTime EndDateTime = DateTime.Now;
            TimeSpan span = EndDateTime - StartDateTime;
            int ms = (int)span.TotalMilliseconds;
            return ms;
        }

        //Same as scenario2 but instead of explicitly creating 7 threads, use parallel 
        //to dictate task creation
        private double Scenario3()
        {
            DateTime StartDateTime = DateTime.Now;

            var producers = Task.Run(() =>
            {
                Parallel.ForEach(documentSource, (docId) =>
                {
                    this.itemsDequeued.TryAdd(docId, 0);
                    var p1Result = this.ReadDocument(docId);
                    var p2Result = this.ProcessDocument(p1Result);
                    bc.Add(p2Result);
                });
            });
            
            var consumer = Task.Run(() =>
            {
                foreach (var item in bc.GetConsumingEnumerable())
                {
                    this.SaveDocument(item);
                }
            });

            producers.Wait();
            bc.CompleteAdding();
            consumer.Wait();

            DateTime EndDateTime = DateTime.Now;
            TimeSpan span = EndDateTime - StartDateTime;
            var ms = span.TotalMilliseconds;
            return ms;
        }

        //Reading the document takes cpu cycles represented by Thread sleeping
        public int ReadDocument(int documentId)
        {
            Thread.Sleep(this.readDocumentLatencyMS);
            return documentId;
        }

        public int ProcessDocument(int documentId)
        {
            Thread.Sleep(this.processDocumentLatencyMS);
            return documentId;
        }

        //A process that wouldn't do well in Parallel.  Which is why there is a lock
        //An example of this would be saving a document since the HDD needle
        //has to move constantly so doing this 1 by 1 improves performance        
        private void SaveDocument(int documentId)
        {
            lock(this._locker)
            {
                Thread.Sleep(this.SaveDocumentLatencyMS);
            }          
        }
    }    
}

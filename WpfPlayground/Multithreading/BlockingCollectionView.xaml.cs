using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfUtilities.Collections;

namespace WpfPlayground.Multithreading
{
    /* Concurrent adding and taking of items from multiple threads
     * Bounded: so you can set a limit to how many items are in the Collection
     * Will block producer thread when capacity reached.  When an item is taken out, 
     * thread will unblock and add the item (it's like being queued)
     * prevents the producing threads from moving too far ahead of the consuming threads
     * Insertion and removal operations that block when collection is empty or full.
     * 
     * Multiple consumers can remove items concurrently, and if the collection becomes empty, 
     * the consuming threads will block until a producer adds an item
     *  A producing thread can call CompleteAdding to indicate that no more items will be added. 
     *  Consumers monitor the IsCompleted property to know when the collection is empty and no 
     *  more items will be added.
     *  
     *  You can also use a different underlying collection.  Default is ConcurrentQueue
     *  
     *  Using Many BlockingCollections As One
     *  For scenarios in which a consumer needs to take items from multiple collections 
     *  simultaneously, you can create arrays of BlockingCollection<T> and use the static 
     *  methods such as TakeFromAny and AddToAny that will add to or take from any of the 
     *  collections in the array. If one collection is blocking, the method immediately 
     *  tries another until it finds one that can perform the operation. 
     */

    public partial class BlockingCollectionView : Window
    {
        public Queue<int> ItemsToAddQueue { get; set; }
        public BlockingCollection<int> ItemsQueue { get; set; }
        public List<int> ItemsDequeued { get; set; }

        public BlockingCollectionView()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.ItemsToAddQueue = new Queue<int>(Enumerable.Range(1, 10));
                        
            this.ItemsQueue = new BlockingCollection<int>(3);
            this.ItemsDequeued = new List<int>();
        }

        private void AddAllToQueueButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() => {
                {
                    while(this.ItemsToAddQueue.Any())
                    {
                        var item = this.ItemsToAddQueue.Dequeue();
                        this.ItemsQueue.Add(item);
                    }
                }
            });
        }

        private void EnqueueButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                var item = this.ItemsToAddQueue.Dequeue();
                this.ItemsQueue.Add(item);
            });
        }

        private void DeQueueButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(()=> {
                if (!this.ItemsQueue.IsCompleted)
                {
                    var item = this.ItemsQueue.Take();
                    this.ItemsDequeued.Add(item);
                }
            });
        }

        //GetConsumingEnumerable iterates the collection while removing the item
        //when empty, it blocks and continues when data gets added
        private void RemoveAllFromBlockingCollection()
        {
            foreach (var item in this.ItemsQueue.GetConsumingEnumerable())
            {                
            }
        }

    }
}

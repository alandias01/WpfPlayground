using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace WpfPlayground.DesignPatterns.Behavioral
{
    /* Rx is about how to react to changes and create data flows that depend on them
     * 
     */

    public class DataService
    {
        public void ExecuteAsync(Action<string> callback)
        {
            new Thread(() =>
            {
                for (int i = 0; i < 3; i++)
                {
                    var uniqueData = DateTime.Now.Millisecond.ToString();
                    callback(uniqueData);
                    Thread.Sleep(2000);
                }
            }).Start();
        }
    }

    public class ObservableRx
    {
        //private readonly IObservable<int> _source = new Observable<int>();
        public int MyPropertya { get; set; }
        public ObservableRx()
        {
            var ds = new DataService();
            ds.ExecuteAsync((msg) =>
            {
                Debug.WriteLine(msg);
            });
        }

        //public void Subs(IObserver<int> observer, IScheduler)
        //{
        //    _source
        //        .Do(observer)
        //        .ObserveOn()
        //        .Subscribe(m => observer.OnNext(m), observer.OnError, observer.OnCompleted);

        //}

        public void GetData(IObservable<int> obs)
        {
            obs.Subscribe(x => DoSomething(x));
        }

        private void DoSomething(int x)
        {
            Console.WriteLine(x);
        }

        /* You return an Observable so that the person can subscribe to a stream
         */
        //private IObservable<object> GetStream()
        //{
        //    //Observable.Create(x=>x.)
        //}
    }
}

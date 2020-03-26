using System;
using System.Collections.Generic;
using System.Net.Http;
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
using Prism.Commands;
using Prism.Mvvm;

namespace WpfPlayground.Multithreading
{
    /*
    2 ways to use async await 
        I/O Bound
            When you request data from a DB, from web, from hard drive
        CPU Bound
            Doing intense calculation
    
    You dont want UI thread to hang so we await operation
    For IO, you await operation.  You don't start another thread.  You make a call to fetch the data from outside world
    The outside world has to return the data to you. It may take 30 sec.  In that 30 sec, you should not block the UI thread

    For CPU bound you await operation that is started on another thread

    Once code reaches await, code gets suspended there and returns control to the calling thread.  
    When data comes back, await continues from that point

    https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/index
    The async and await keywords don't cause additional threads to be created. 
    Async methods don't require multithreading because an async method doesn't run on its own thread. 
    The method runs on the current synchronization context and uses time on the thread only when the method is active. 

    */

    /* What does this program do?
     * You have 2 buttons
     *  *Fetch Json data from a REST Api, a url that returns json
     *  *Cancel the async fetch
     * 
     * When You press Fetch, you pas url and cancellation token
     * It says awaiting, once data comes in, updates the textbox
     * If you press cancel, token gets fired and exception bubbles up and exception messages shows in textbox     
     */
    public partial class AsyncAwait01 : Window
    {
        public AsyncAwait01()
        {
            this.InitializeComponent();
            this.DataContext = new AsyncAwait01VM();
        }
    }

    public class AsyncAwait01VM : BindableBase
    {
        private string textBlock1Text;
        public string TextBlock1Text { get => textBlock1Text; set => this.SetProperty(ref textBlock1Text, value); }

        CancellationTokenSource Button01CancellationSource = new CancellationTokenSource();

        public DelegateCommand Button01Command { get; set; }
        public DelegateCommand Button01CancelCommand { get; set; }

        public AsyncAwait01VM()
        {
            this.Button01Command = new DelegateCommand(Button1, CanButton1);
            this.Button01CancelCommand= new DelegateCommand(Button1Cancel);
            this.TextBlock1Text = "Empty";
        }

        private async void Button1()
        {
            try
            {
                if(Button01CancellationSource.IsCancellationRequested)
                {
                    Button01CancellationSource = new CancellationTokenSource();
                }

                this.TextBlock1Text = "awaiting...";
                string url = "https://data.cityofnewyork.us/resource/dx8z-6nev.json";
                
                var task = this.GetFirst5CharFromApiAsync(url, Button01CancellationSource.Token);
                this.TextBlock1Text = await task;
            }
            catch (Exception e)
            {
                this.TextBlock1Text = e.Message;
            }            
        }

        private void Button1Cancel() => this.Button01CancellationSource.Cancel(); //Should wrap in try
        
        /// <summary>
        /// The way this Task was designed was what service can we provide to the CONSUMER.
        /// We are not the consumer so we are not going to update the textbox or do anything to 
        /// the result.
        /// The service we provide is get the First 5 characters from the Rest call.
        /// If the user wants only the first 3, they can do that.
        /// We provide them a Task, and they can do what ever they want with it.
        /// They would await this Task to get the result.
        /// </summary>
        /// <param name="url">Restful endpoint</param>
        /// <param name="cancellationToken">Cancel button triggers this</param>
        /// <returns></returns>
        private async Task<string> GetFirst5CharFromApiAsync(string url, CancellationToken cancellationToken)
        {
            string first5Char = string.Empty;
            try
            {
                //You should check url for null or empty
                var client = new HttpClient();
                var responseMessage = await client.GetAsync(url, cancellationToken);
                var rawString = await responseMessage.Content.ReadAsStringAsync();
                first5Char = rawString.Substring(0, 5);
            }
            catch (Exception e)
            {                
                throw;
            }
            
            return first5Char;
        }

        private bool CanButton1() => true;

        /***********************BELOW UNRELATED TO PROGRAM***********************/
        
        //This method returns a Task without you doing it explicitly
        //Since this is an async method of Task
        //If this was Task<T>, then we would have to return T
        private async Task DoesntReturnValue()
        {
            await Task.Delay(1000);
        }

        //Lets say you hav a class that provides a service, (A method that does long running cpu calculation).  
        //The service is CPU intensive
        //Do you want to provide a Task that puts it on a new Thread or give them a plain function?
        //You provide them a plain function.  The CONSUMER decides if they want to wrap it in a Task

        private string LongRunningJob() => "Alot of work";

        //You are the CONSUMER below
        private async void MyMainMethod()
        {
            var result1 = await Task<string>.Run(this.LongRunningJob);
            //var result1 = await Task<string>.Run(() => this.LongRunningJob());     //Another way to write

            //You can also wrap LongRunningJob() in a Task<string> as a consumer but don't do this as a service provider

            //Here we have LongRunningJobTask1() wrapped in a Task and we await it here
            string result21 = await this.LongRunningJobTask1();
            //or
            Task<string> localTask = this.LongRunningJobTask1();
            //do something
            string result22 = await localTask;

            //In this way, LongRunningJobTask2() has await in it but returning right afterwards
            //This doesn't make sense unless you do something with the awaited result in the method            
            string result3 = await LongRunningJobTask2();
        }

        private Task<string> LongRunningJobTask1()
        {
            return Task<string>.Run(this.LongRunningJob);
        }

        //You shouldn't return await unless you are going to write meaningful code between the 2 lines
        //Otherwise you can just return the Task and let user await
        private async Task<string> LongRunningJobTask2()
        {
            var res = await Task<string>.Run(this.LongRunningJob);
            //You should do something useful here if you're going to await, otherwise return Task
            return res;            
        }
    }
}

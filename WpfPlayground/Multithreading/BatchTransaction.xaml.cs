using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;

namespace WpfPlayground.Multithreading
{
    /// <summary>
    /// Interaction logic for BatchTransaction.xaml
    /// </summary>
    public partial class BatchTransaction : Window, INotifyPropertyChanged
    {
        List<Action> actions = new List<Action>();
        const double interval = 6000;
        Timer timer = new Timer(interval);

        private string logWindow;

        public string LogWindow
        {
            get => this.logWindow;
            set
            {
                logWindow = value;
                this.OnPropertyChanged();
                this.txtLogWindow.ScrollToEnd();
            }
        }

        public BatchTransaction()
        {
            this.InitializeComponent();
            this.DataContext = this;
            timer.Elapsed += this.Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.Logger("Timer_Elapsed");
                Action action = () =>
                {
                    for (int i = 0; i < this.actions.Count; i++)
                    {
                        this.actions[i].Invoke();
                    }
                    actions.Clear();
                };
                Dispatcher.BeginInvoke(action);
            }
            catch (Exception ex)
            {
                this.Logger(ex);
            }
        }

        void BatchAddTransaction(Action item)
        {
            if (timer.Enabled)
            {
                this.actions.Add(item);
            }
            else
            {
                timer.Start();
                this.actions.Add(item);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            try
            {

                Action trans = () => { this.Logger("Hello");};
                this.BatchAddTransaction(trans);
            }
            catch (Exception ex)
            {
                this.Logger(ex);
            }
        }

        private void Logger(string msg) => this.LogWindow += (DateTime.Now.ToString() + ": " + msg + Environment.NewLine);
        private void Logger(Exception e) => this.LogWindow += (DateTime.Now.ToString() + ": " + e.ToString() + Environment.NewLine);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

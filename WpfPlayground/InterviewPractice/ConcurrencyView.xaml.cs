using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPlayground.InterviewPractice
{
    /// <summary>
    /// Interaction logic for ConcurrencyView.xaml
    /// </summary>
    public partial class ConcurrencyView : UserControl
    {
        public ConcurrencyView()
        {
            InitializeComponent();
        }
    }

    public enum ConcurrencyViewModelEventArgsEnum
    {
        ProducerConsumer
    }

    public class ConcurrencyViewModelEventArgs : IViewModelEventArgs
    {
        public ConcurrencyViewModelEventArgsEnum ConcurrencyViewModelType;

        public ConcurrencyViewModelEventArgs(ConcurrencyViewModelEventArgsEnum ConcurrencyViewModelType)
        {
            this.ConcurrencyViewModelType = ConcurrencyViewModelType;
        }
    }

    public class ConcurrencyViewModel : ViewModelBase<ConcurrencyViewModelEventArgs>
    {
        private int BusyCpuLatency = 100;
        public ConcurrencyViewModel(ConcurrencyViewModelEventArgs args) : base(args) => this.Start(args);

        public override void Start(ConcurrencyViewModelEventArgs args)
        {
            try
            {
                switch (args.ConcurrencyViewModelType)
                {
                    case ConcurrencyViewModelEventArgsEnum.ProducerConsumer:
                        this.ProducerConsumer();
                        break;
                    default:
                        this.ProducerConsumer();
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ProducerConsumer()
        {


        }
        private void BusyCpu(int timeToSleep) => Thread.Sleep(timeToSleep);
    }
}

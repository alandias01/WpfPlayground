using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfPlayground.DesignPatterns
{
    /// <summary>
    /// Interaction logic for ObservableRxView.xaml
    /// </summary>
    public partial class ObservableRxView : Window
    {
        public ObservableRxView()
        {
            InitializeComponent();
            this.DataContext = new ObservableRxViewVM();
        }
    }

    public class ObservableRxViewVM : BindableBase
    {
        private string name;
        private ObservableRxViewVM vm;
        DispatcherTimer timer;
        Random random = new Random();

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public DelegateCommand RunCommand { get; set; }

        public ObservableRxViewVM()
        {
            RunCommand = new DelegateCommand(RunCommandExecute);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Name = random.Next(1, 10).ToString();
        }

        private void RunCommandExecute()
        {
            Name = "Alan";
        }
    }
}

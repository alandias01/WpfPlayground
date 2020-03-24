using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;

namespace WpfPlayground.DependencyInjection
{
    /// <summary>
    /// This window has 2 buttons, new window and publish
    /// Window subscribes to MSE object and when it receives a msg, adds to collection
    /// which is bound to DataGrid
    /// </summary>
    public partial class EventAggregatorWindow : Window
    {
        private IEventAggregator _ea;

        public ObservableCollection<StringHolder> StringList { get; set; }

        public EventAggregatorWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            this.StringList = new ObservableCollection<StringHolder>();
            _ea = UnityManager.container.Resolve<IEventAggregator>();
            _ea.GetEvent<MSE>().Subscribe(MessageReceived);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new EventAggregatorWindow().Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {                        
            _ea.GetEvent<MSE>().Publish("Hello");            
        }

        private void MessageReceived(string msg)
        {
            this.StringList.Add(new StringHolder { Msg = msg });
        }
    }
    
    public class MSE : PubSubEvent<string>
    {
    }

    public class StringHolder
    {
        public string Msg { get; set; }
    }
}

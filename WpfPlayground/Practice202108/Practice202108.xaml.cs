using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/* Interview Preparation
 * Pane wrapper to handle window events in the VM
 * ViewModel Base
 * Dock panel with top, left with links, right is where view gets populated
 * Right side gets populated on datatemplate.
 * Right side is a user control that binds to a viewmodel
 * Left side expander.  Style trigger isMouseOver to change bgcolor.  TextBlock with mouse binding left click
 * Delegate command which launches the view
 * 
 * Data trigger if value > 20, change bg
 * 
 * 
 * 
 */


namespace WpfPlayground.Practice202108
{
    /// <summary>
    /// Interaction logic for Practice202108.xaml
    /// </summary>
    public partial class Practice202108 : Window
    {
        public Practice202108()
        {
            InitializeComponent();
            //this.DataContext = new Practice202108ViewModel();
        }
    }


    public class BulkObservableCollection<T> : ObservableCollection<T>
    {
        private bool shouldRaiseCollectionChanged = true;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.shouldRaiseCollectionChanged)
                base.OnCollectionChanged(e);
        }

        public void AddRange(IEnumerable<T> list)
        {
            this.shouldRaiseCollectionChanged = false;
            foreach (T item in list)
            {
                this.Add(item);
            }
            this.shouldRaiseCollectionChanged = true;
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    public class Practice202108ViewModel : BindableBase
    {
        BulkObservableCollection<string> Oc = new BulkObservableCollection<string>();
        public DelegateCommand ShowDataCommand { get; set; }

        public Practice202108ViewModel()
        {
            Oc.CollectionChanged += this.Oc_CollectionChanged;
            ShowDataCommand = new DelegateCommand(showData);
        }

        private void showData()
        {
            Oc.AddRange(new List<string> { "Alan", "Balan" });            
        }

        private void Oc_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }

        private async Task<string> GetFirst5Char(string str)
        {
            var result = str.Substring(0, 5);
            await Task.Delay(500);
            return result;
        }
    }
}

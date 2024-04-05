using System;
using System.Collections.Generic;
using System.Linq;
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
using Prism.Commands;
using Prism.Mvvm;
using WpfUtilities;

namespace WpfPlayground.Binding
{
    /// <summary>
    /// Interaction logic for Binding.xaml
    /// </summary>
    public partial class INotify : Window
    {
        public INotify()
        {
            InitializeComponent();
            this.DataContext = new INotifyVM();
        }
    }

    public class INotifyVM : BindableBase
    {
        private string name1;
        private string name2;
        private string name3;
        private string name4;

        public string Name1 { get => name1; set => name1 = value; } // UI sees Alan but doesnt get updated afterwards
        public string Name2 { get => name2; set => SetProperty(ref name2, value); } // UI sees Dias, Command updates BC of INotify
        public string Name3 { get => name3; set => SetProperty(ref name3, value); } // Textbox does not update property in realtime
        public string Name4 { get => name4; set => SetProperty(ref name4, value); } // Textbox updates property in realtime, updateSourcetrigger

        public DelegateCommand ChangeName { get; set; }
        public INotifyVM()
        {
            this.Name1 = "Alan";
            this.Name2 = "Dias";
            this.Name3 = "3";
            this.Name4 = "4";

            ChangeName = new DelegateCommand(() => {
                Name1 = "A";
                Name2 = "B";            
            });
        }
    }
}

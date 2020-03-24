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
using WpfUtilities;

namespace WpfPlayground.Binding
{
    /// <summary>
    /// Interaction logic for MasterDetail.xaml
    /// </summary>
    public partial class MasterDetail : Window
    {
        public MasterDetail()
        {
            InitializeComponent();
            this.DataContext = new MasterDetailVM();
        }
    }

    public class MasterDetailVM
    {
        public ObservableCollection<Employee> EmpList { get; set; }
        
        public MasterDetailVM()
        {
            this.EmpList = Employee.Load();
        }
    }
}

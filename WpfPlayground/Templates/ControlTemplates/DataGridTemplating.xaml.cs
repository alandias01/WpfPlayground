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

namespace WpfPlayground.Templates.ControlTemplates
{
    /// <summary>
    /// Interaction logic for DataGridTemplating.xaml
    /// </summary>
    public partial class DataGridTemplating : Window
    {
        public DataGridTemplating()
        {
            InitializeComponent();
            this.DataContext = new DataGridTemplatingVM();
        }
    }

    public class DataGridTemplatingVM
    {
        public ObservableCollection<Employee> EmpList { get; set; }

        public DataGridTemplatingVM()
        {
            this.EmpList = Employee.Load();
        }
    }
}

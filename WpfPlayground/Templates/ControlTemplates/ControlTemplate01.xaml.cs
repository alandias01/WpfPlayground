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
using System.Linq;

namespace WpfPlayground.Templates.ControlTemplates
{
    /// <summary>
    /// Interaction logic for ControlTemplate01.xaml
    /// </summary>
    public partial class ControlTemplate01 : Window
    {
        public ControlTemplate01()
        {
            InitializeComponent();
            this.DataContext = new ControlTemplate01ViewModel();
        }        
    }

    public class ControlTemplate01ViewModel
    {
        public Employee EmpSingle { get; set; }
        public ObservableCollection<Employee> EmpList { get; set; }
                
        public ControlTemplate01ViewModel()
        {
            this.EmpList = Employee.Load();
            this.EmpSingle = this.EmpList.FirstOrDefault();
        }
    }
}

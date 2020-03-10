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

namespace WpfPlayground.Behaviors
{
    /// <summary>
    /// Interaction logic for Behavior01.xaml
    /// </summary>
    public partial class Behavior01 : Window
    {
        public Behavior01()
        {
            this.InitializeComponent();
            this.DataContext = new Behavior01VM();            
        }
    }

    public class Behavior01VM
    {        
        public List<Employee> EmployeeList { get; set; }
        public Employee EmployeeSelectedItem { get; set; }
        public List<Employee> EmployeeSelectedItems { get; set; }
        
        public Behavior01VM()
        {
            this.EmployeeList = new List<Employee>(Employee.Load());            
            this.EmployeeSelectedItems = new List<Employee>();
        }
    }

    /// <summary>
    ///   You have to subclass since xaml doesn't take generics
    /// </summary>
    public class MyListBoxBehavior : ListBoxSelectionBehavior<Employee>
    {
    }
}

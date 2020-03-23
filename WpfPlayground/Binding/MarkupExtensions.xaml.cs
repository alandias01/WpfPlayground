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
using WpfUtilities;

namespace WpfPlayground.Binding
{
    /// <summary>
    /// Interaction logic for MarkupExtensions.xaml
    /// </summary>
    public partial class MarkupExtensions : Window
    {
        public MarkupExtensions()
        {
            this.InitializeComponent();
            //this.DataContext = new MarkupExtensionsVM();
        }
    }

    public class MarkupExtensionsVM
    {
        public Employee EmpSingle { get; set; }

        public static readonly string EmpStatic = "Alan Static";

        public string SetInXaml { get; set; }

        public MarkupExtensionsVM()
        {
            this.EmpSingle = new Employee("Alan", 30);
        }
    }

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

        }

        public MarkupExtensionsVM MarkUpVM
        {
            get
            {
                return new MarkupExtensionsVM();
            }
        }
    }
}

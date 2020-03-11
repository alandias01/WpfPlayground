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

namespace WpfPlayground.TriggersConverters
{
    /// <summary>
    /// Interaction logic for Trigger01.xaml
    /// </summary>
    public partial class Trigger01 : Window
    {
        public Trigger01()
        {
            this.InitializeComponent();
            this.DataContext = new Trigger01VM();
        }
    }

    public class Trigger01VM
    {
        public Employee EmployeeSingle { get; set; }

        public Trigger01VM()
        {
            EmployeeSingle = new Employee("Alan", 21);
        }
    }

    public class LessThanTwentyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int tmpage = System.Convert.ToInt32(value);
            return tmpage < 20;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return true;
        }
    }

    public class WithinRangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int range = System.Convert.ToInt32(value);
            return range >= 300 && range <= 400;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return true;
        }
    }
}

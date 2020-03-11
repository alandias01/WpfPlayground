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

namespace WpfPlayground.TriggersConverters
{
    /// <summary>
    ///   We gave the window the name "win"
    ///   Bind the window height to the text box so you can change the value by resizing
    ///   Value converter changes the value to Money
    /// </summary>
    public partial class ValueConverter01 : Window
    {
        public ValueConverter01()
        {
            this.InitializeComponent();
        }
    }

    public class NumberToMoneyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double s = (double)value;
            return s.ToString("C");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}

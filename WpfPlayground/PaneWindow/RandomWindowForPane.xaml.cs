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

namespace WpfPlayground.PaneWindow
{
    /// <summary>
    /// Interaction logic for RandomWindowForPane.xaml
    /// </summary>
    public partial class RandomWindowForPane : PaneWindowBase
    {
        public RandomWindowForPane() : base(new RandomWindowForPaneViewModel())
        {
            InitializeComponent();
        }
    }

    public class RandomWindowForPaneViewModel : PaneWindowBaseViewModel
    {
        public RandomWindowForPaneViewModel()
        {            
        }
    }
}

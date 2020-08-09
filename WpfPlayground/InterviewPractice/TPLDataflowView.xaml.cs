using Prism.Mvvm;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfPlayground.InterviewPractice
{
    /// <summary>
    /// Interaction logic for TPLDataflowView.xaml
    /// </summary>
    public partial class TPLDataflowView : UserControl
    {
        public TPLDataflowView()
        {
            this.InitializeComponent();
        }
    }

    public class TPLDataflowViewModel : BindableBase
    {

    }
}

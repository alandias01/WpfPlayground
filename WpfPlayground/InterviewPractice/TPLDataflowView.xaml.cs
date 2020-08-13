using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        private string output;

        public string Output { get => output; set => this.SetProperty(ref output, value); }
        public TPLDataflowViewModel()
        {
            this.Output = "Hello";
            //var ablock = new ActionBlock<int>(input => Console.WriteLine(input));
        }
    }
}

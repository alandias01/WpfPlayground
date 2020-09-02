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
    public partial class TPLDataflowView : UserControl
    {
        public TPLDataflowView()
        {
            this.InitializeComponent();
        }
    }

    public class TPLDataflowViewModelEventArgs : IViewModelEventArgs { }

    public class TPLDataflowViewModel : ViewModelBase<TPLDataflowViewModelEventArgs>
    {
        private string output;

        public TPLDataflowViewModel(TPLDataflowViewModelEventArgs args) : base(args) => this.Start(args);

        public override void Start(TPLDataflowViewModelEventArgs arg)
        {
            this.Output = "Hello";
            var ablock = new ActionBlock<string>(ABlockFunc);
            ablock.Post(" Hi");
        }

        public string Output { get => output; set => this.SetProperty(ref output, value); }

        private void ABlockFunc(string input)
        {
            Output += input;
        }

        /* PCQ
         * Create a list of data items to process
         * Create a process that works on the data that takes x time
         * 
         */
    }
}

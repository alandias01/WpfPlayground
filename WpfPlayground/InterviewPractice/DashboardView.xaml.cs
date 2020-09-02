using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            this.InitializeComponent();
        }
    }

    public class DashboardViewModelEventArgs : IViewModelEventArgs { }

    public class DashboardViewModel : ViewModelBase<DashboardViewModelEventArgs>
    {
        public DashboardViewModel(DashboardViewModelEventArgs args) : base(args) => this.Start(args);
        
        public override void Start(DashboardViewModelEventArgs arg)
        {
        }
    }
}

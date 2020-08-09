using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPlayground.InterviewPractice
{
    /// <summary>
    /// Interaction logic for InterviewMainWindow.xaml
    /// </summary>
    public partial class InterviewMainWindow : Window
    {
        public InterviewMainWindow()
        {
            InitializeComponent();
            this.DataContext = new InterviewMainWindowViewModel();
        }
    }

    public class InterviewMainWindowViewModel : BindableBase
    {
        private BindableBase mainAreaViewModel;
        private List<BindableBase> viewmodels = new List<BindableBase>();
        public BindableBase MainAreaViewModel { get => mainAreaViewModel; set => this.SetProperty(ref mainAreaViewModel, value); }
        public DelegateCommand ShowStockDashboardCommand { get; set; }
        public DelegateCommand RunTPLDataflowCommand { get; set; }

        public InterviewMainWindowViewModel()
        {
            this.ShowStockDashboardCommand = new DelegateCommand(ShowStockDashboard);
            this.RunTPLDataflowCommand = new DelegateCommand(RunTPLDataflow);
        }

        private void ShowStockDashboard()
        {
            this.SetMainAreaViewModel<DashboardViewModel>();
        }

        private void RunTPLDataflow()
        {
            this.SetMainAreaViewModel<TPLDataflowViewModel>();
        }

        private void SetMainAreaViewModel<T>() where T : BindableBase, new()
        {
            var vm = this.MainAreaViewModel as T;
            if (vm == null)
            {
                var foundObjectOfType = this.viewmodels.OfType<T>().FirstOrDefault();
                if (foundObjectOfType != null)
                {
                    this.MainAreaViewModel = foundObjectOfType;
                }
                else
                {
                    var newVM = new T();
                    this.MainAreaViewModel = newVM;
                    this.viewmodels.Add(newVM);
                }
            }
        }
    }
}

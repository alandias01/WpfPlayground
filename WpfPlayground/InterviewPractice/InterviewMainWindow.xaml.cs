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
using WpfPlayground.InterviewPractice.Algorithms;

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
        public DelegateCommand RunProducerConsumerCommand { get; set; }
        public DelegateCommand RunAlgoProblemsCommand { get; set; }
        public DelegateCommand RunQuicksortCommand { get; set; }
        public DelegateCommand RunBinarySearchCommand { get; set; }
        public DelegateCommand RunBubbleSortCommand { get; set; }
        public DelegateCommand RunGraphCommand { get; set; }
        
        public InterviewMainWindowViewModel()
        {
            this.ShowStockDashboardCommand = new DelegateCommand(ShowStockDashboard);
            this.RunTPLDataflowCommand = new DelegateCommand(RunTPLDataflow);
            this.RunAlgoProblemsCommand = new DelegateCommand(RunAlgoProblems);
            this.RunQuicksortCommand = new DelegateCommand(RunQuickSort);
            this.RunProducerConsumerCommand = new DelegateCommand(RunConcurrency);            
            this.RunBinarySearchCommand = new DelegateCommand(RunBinarySearch);
            this.RunBubbleSortCommand = new DelegateCommand(RunBubbleSort);
            this.RunGraphCommand = new DelegateCommand(RunGraph);
        }

        private void ShowStockDashboard()
        {
            var args = new DashboardViewModelEventArgs();
            this.SetMainAreaViewModel<DashboardViewModel, DashboardViewModelEventArgs>(args);
        }

        private void RunTPLDataflow()
        {
            var args = new TPLDataflowViewModelEventArgs();
            this.SetMainAreaViewModel<TPLDataflowViewModel, TPLDataflowViewModelEventArgs>(args);
        }

        private void RunAlgoProblems() => this.RunAlgorithms(AlgorithmsViewModelEventArgsEnum.AlgoProblems);
        private void RunQuickSort() => this.RunAlgorithms(AlgorithmsViewModelEventArgsEnum.quickSort);
        private void RunBinarySearch() => this.RunAlgorithms(AlgorithmsViewModelEventArgsEnum.binarySearch);
        private void RunBubbleSort() => this.RunAlgorithms(AlgorithmsViewModelEventArgsEnum.bubbleSort);
        private void RunGraph() => this.RunAlgorithms(AlgorithmsViewModelEventArgsEnum.graph);

        private void RunConcurrency()
        {
            var args = new ConcurrencyViewModelEventArgs(ConcurrencyViewModelEventArgsEnum.ProducerConsumer);
            this.SetMainAreaViewModel<ConcurrencyViewModel, ConcurrencyViewModelEventArgs>(args);
        }

        private void RunAlgorithms(AlgorithmsViewModelEventArgsEnum eventArgsEnum)
        {
            var args = new AlgorithmsViewModelEventArgs(eventArgsEnum);
            this.SetMainAreaViewModel<AlgorithmsViewModel, AlgorithmsViewModelEventArgs>(args);
        }

        private void SetMainAreaViewModel<T, U>(U args) where T : ViewModelBase<U> where U : IViewModelEventArgs
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
                    var newVM = (T)Activator.CreateInstance(typeof(T), args);
                    this.MainAreaViewModel = newVM;
                    this.viewmodels.Add(newVM);
                }
            }
            else
            {
                vm.Start(args);
            }
        }
    }
}

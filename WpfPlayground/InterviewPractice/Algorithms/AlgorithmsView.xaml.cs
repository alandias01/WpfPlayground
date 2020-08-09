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

namespace WpfPlayground.InterviewPractice.Algorithms
{
    /// <summary>
    /// Interaction logic for AlgorithmsView.xaml
    /// </summary>
    public partial class AlgorithmsView : UserControl
    {
        public AlgorithmsView()
        {
            InitializeComponent();
        }
    }

    public enum AlgorithmsViewModelEventArgsEnum
    {
        AlgoProblems,
        quickSort,
        binarySearch,
        bubbleSort,
        graph
    }


    public class AlgorithmsViewModelEventArgs : IViewModelEventArgs
    {
        public AlgorithmsViewModelEventArgsEnum AlgorithmsViewModelType;

        public AlgorithmsViewModelEventArgs(AlgorithmsViewModelEventArgsEnum algorithmsViewModelType)
        {
            this.AlgorithmsViewModelType = algorithmsViewModelType;
        }
    }

    public class AlgorithmsViewModel : ViewModelBase<AlgorithmsViewModelEventArgs>
    {
        public AlgorithmsViewModel(AlgorithmsViewModelEventArgs args) : base(args) => this.Start(args);

        public override void Start(AlgorithmsViewModelEventArgs args)
        {
            switch (args.AlgorithmsViewModelType)
            {
                case AlgorithmsViewModelEventArgsEnum.AlgoProblems:
                    new AlgoProblems();
                    break;
                case AlgorithmsViewModelEventArgsEnum.quickSort:
                    new Quicksort();
                    break;
                case AlgorithmsViewModelEventArgsEnum.binarySearch:
                    new BinarySearch();
                    break;
                case AlgorithmsViewModelEventArgsEnum.bubbleSort:
                    new BubbleSort();
                    break;
                case AlgorithmsViewModelEventArgsEnum.graph:
                    new Graph();
                    break;
                default:
                    new BinarySearch();
                    break;
            }
        }
    }
}

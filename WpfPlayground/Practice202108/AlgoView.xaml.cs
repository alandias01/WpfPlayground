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

namespace WpfPlayground.Practice202108
{
    /// <summary>
    /// Interaction logic for AlgoView.xaml
    /// </summary>
    public partial class AlgoView : UserControl
    {
        public AlgoView()
        {
            InitializeComponent();
        }
    }

    public enum AlgoViewModelEnum
    {
        Step1,
        Step2
    }

    public interface IViewModelBase<TEnum>
    {
        void Start(TEnum args);
    }

    public class AlgoViewModel : IViewModelBase<AlgoViewModelEnum>
    {
        public AlgoViewModel()
        {

        }

        public void Start(AlgoViewModelEnum args)
        {
            switch (args)
            {
                case AlgoViewModelEnum.Step1:
                    this.Step1();
                    break;
                case AlgoViewModelEnum.Step2:
                    this.Step2();
                    break;
            }
        }

        private void Step1()
        {

        }

        private void Step2()
        {

        }

    }
}

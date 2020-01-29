using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.Windows;
using System.Windows.Input;

using Prism.Commands;
using Prism.Mvvm;
using WpfUtilities;

namespace WpfPlayground.Commands
{    
    public partial class CommandWindow : Window
    {
        public CommandWindow()
        {
            this.InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }

    public class MainWindowViewModel : BindableBase
    {
        private Employee currentItem;
                
        public ObservableCollection<Employee> EmpList { get; set; }

        public DelegateCommand<Employee> ShowCurrentCommand { get; set; }
        public DelegateCommand ButtonUpdateCommand { get; set; }
        public DelegateCommand<IList> ShowSelectedItemsCommand { get; set; }
                
        public Employee CurrentItem
        {
            get { return currentItem; }
            set
            {
                currentItem = value;
                ShowCurrentCommand.RaiseCanExecuteChanged();
                this.SetProperty(ref currentItem, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public MainWindowViewModel()
        {
            EmpList = Employee.Load();
            ShowCurrentCommand = new DelegateCommand<Employee>(Add, CanAdd);
            ButtonUpdateCommand = new DelegateCommand(Update);
            ShowSelectedItemsCommand = new DelegateCommand<IList>(ShowSelectedItems);
        }

        private void Add(Employee e)
        {
            MessageBox.Show(e.Name);
        }

        private bool CanAdd(Employee e)
        {
            if (CurrentItem == null)
            {
                return false;
            }
            else
                //return e.Name.ToLower() == "alan";
                return CurrentItem.Name.ToLower() == "alan";
        }

        private void Update()
        {
            foreach (var item in EmpList)
            {
                item.Age += 1;
            }
        }

        private void ShowSelectedItems(IList s)
        {

        } 
    }
}

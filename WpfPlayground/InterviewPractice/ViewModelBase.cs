using Prism.Mvvm;
using System;

namespace WpfPlayground.InterviewPractice
{
    public interface IViewModelEventArgs { }

    public abstract class ViewModelBase<T> : BindableBase where T : IViewModelEventArgs
    {
        public ViewModelBase(T args)
        {
        }

        public abstract void Start(T args);
    }
}

using Prism.Mvvm;

namespace WpfPlayground.Binding
{
    public class ItemWithNotificationPrism : BindableBase
    {
        private string name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
    }
}

using Prism.Mvvm;

namespace WpfPlayground.INotify
{
    public class ItemWithNotificationPrism : BindableBase
    {
        private string name;

        public string Name
        {
            get => name;
            set => this.SetProperty(ref name, value);            
        }
    }
}

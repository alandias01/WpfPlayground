using Prism.Mvvm;

namespace WpfPlayground.INotify
{
    public class ItemWithNotificationPrism : BindableBase
    {
        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                this.SetProperty(ref name, value);
            }
        }
    }
}

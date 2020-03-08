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
                name = value;  //may not need this line
                this.SetProperty(ref name, value);
            }
        }
    }
}

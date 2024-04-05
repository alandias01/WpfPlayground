using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfPlayground.Binding
{
    public class ItemWithNotification : INotifyPropertyChanged
    {
        private string name1;
        private string name2;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name1
        {
            get => name1;
            set
            {
                name1 = value;
                OnPropertyChanged1("name1");
            }
        }

        public string Name2
        {
            get => name2;
            set
            {
                name2 = value;
                OnPropertyChanged2();
            }
        }

        protected void OnPropertyChanged1(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChanged2([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

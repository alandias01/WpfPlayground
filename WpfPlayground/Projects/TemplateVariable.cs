using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfPlayground.Projects
{
    public class TemplateVariable
    {
        private string _displayText;
        private string _variableType;
        private string _defaultValue;

        public string DisplayText
        {
            get => _displayText;
            set { _displayText = value; OnPropertyChanged(); }
        }

        public string VariableType
        {
            get => _variableType;
            set { _variableType = value; OnPropertyChanged(); }
        }

        public string DefaultValue
        {
            get => _defaultValue;
            set
            {
                _defaultValue = value;
                OnPropertyChanged();
                // Notify parent bindings that depend on RenderedHtml/DiffText
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DefaultValue)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

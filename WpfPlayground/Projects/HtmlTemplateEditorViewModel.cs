using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WpfPlayground.Projects
{
    public class HtmlTemplateEditorViewModel
    {
        private string _templateHtml;

        public string TemplateHtml
        {
            get => _templateHtml;
            set
            {
                if (_templateHtml != value)
                {
                    _templateHtml = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(RenderedHtml));
                    OnPropertyChanged(nameof(DiffText));
                }
            }
        }

        public ObservableCollection<TemplateVariable> Variables { get; } =
            new ObservableCollection<TemplateVariable>
            {
                new TemplateVariable { DisplayText = "username", VariableType = "string", DefaultValue = "Alice" },
                new TemplateVariable { DisplayText = "role", VariableType = "string", DefaultValue = "Admin" }
            };

        public string RenderedHtml
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TemplateHtml))
                    return string.Empty;

                string result = TemplateHtml;
                foreach (var v in Variables)
                {
                    string pattern = @"\[" + Regex.Escape(v.DisplayText) + @"\]";

                    if (!string.IsNullOrEmpty(v.DefaultValue))
                        result = Regex.Replace(result, pattern, v.DefaultValue);
                }
                return result;
            }
        }

        public string DiffText
        {
            get
            {
                if (TemplateHtml == RenderedHtml)
                    return "No differences.";
                return $"--- Template ---\n{TemplateHtml}\n\n--- Rendered ---\n{RenderedHtml}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}

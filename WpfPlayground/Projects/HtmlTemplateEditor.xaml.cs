using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPlayground.Projects
{
    /// <summary>
    /// Interaction logic for HtmlTemplateEditor.xaml
    /// </summary>
    public partial class HtmlTemplateEditor : Window
    {
        private HtmlTemplateEditorViewModel ViewModel => DataContext as HtmlTemplateEditorViewModel;

        public HtmlTemplateEditor()
        {
            InitializeComponent();
            var vm = new HtmlTemplateEditorViewModel
            {
                TemplateHtml = @"<!DOCTYPE html>
                    <html>
                    <head><title>Template</title></head>
                    <body>
                        <h1>Hello [username]!</h1>
                        <p>Your role is [role]</p>
                    </body>
                    </html>"
            };

            DataContext = vm;

            // Ensure preview shows initial content
            Loaded += (s, e) =>
            {
                HtmlPreview.NavigateToString(((HtmlTemplateEditorViewModel)DataContext).RenderedHtml ?? string.Empty);
            };
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HtmlPreview != null && HtmlPreview.IsLoaded)
            {
                HtmlPreview.NavigateToString(ViewModel.RenderedHtml ?? string.Empty);
            }
        }
    }
}

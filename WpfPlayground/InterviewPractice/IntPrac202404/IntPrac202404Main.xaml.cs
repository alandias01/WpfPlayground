using Prism.Mvvm;
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

namespace WpfPlayground.InterviewPractice.IntPrac202404
{

    /* expression bodied members for property, ctor, and method
     * 
     * Resources Section
     * Binding, MarkupExtensions, {source, elementName, path, StaticResource}
     * RelativeSource, AncestorType, Mode, Path
     * 
     * inotify
     * set data context multiple ways
     * 
     * commands
     *  Use a command to get selectedItems
     *  Commandparams
     *  EventToCommand
     *  When left click, run command (input bindings)
     *  Can we make this a style for any rectangle
     * 
     * triggers: Property, data
     *  When mouse property is over any textblock, set bg to red
     * 
     * Converters
     * Bind to list and modify DataTemplate
     * 
     * Modify controls
     *  ContentControl vs ItemsControl
     *  ControlTemplates
     *  What does the contentPresenter do?
     * 
     * Window
     *  Create a shell view composed of looslely coupled reusable view components
     *   Use Grids and stackpanels
     *  Create a bindableBase property that holds a mainVM, when changed, loads correct View using DataTemplate
     *  
     *  Unitycontainer
     *  Event Aggregator
     *  Create messagebus
     *  singleton
     *  
     * MVVM
     * Binding Validation
     *  ValidatesOnDataErrors, <Trigger Property="Validation.HasError" Value="True">
     *  <Binding.ValidationRules><local:CharacterRule/> ,class CharacterRule : ValidationRule
     *  
     *  Built in ValidationRule
     *  <Binding.ValidationRules>
                <ExceptionValidationRule /> // or you can use shortcut attribute: Text="{Binding UserNameValidation02, ValidatesOnExceptions=True }" >
            </Binding.ValidationRules>
     * 
     *  When validation fails, you can use an errorTemplate to change the display
     *  You can also use a trigger to check property Validation.HasError and set tooltip
     *  
     *  You can also use DataErrorValidationRule
     *  <Binding.ValidationRules>
     *      <DataErrorValidationRule /> Shortcut is  ValidatesOnDataErrors=True
     *   checks for errors that are raised by objects that implement the IDataErrorInfo interface
     * 
     * Iterate a collection based on type
     * ObservableCollection, sync context
     * collectionview
     * 
     * Extra
     * 
     * Concurrency
     */

    /// <summary>
    /// Interaction logic for IntPrac202404Main.xaml
    /// </summary>
    public partial class IntPrac202404Main : Window
    {
        public IntPrac202404Main()
        {
            InitializeComponent();
        }
    }

    public class IntPrac202404MainVM : BindableBase
    {
        private string name;

        public string Name { get => name; set => SetProperty(ref name, value); }
    }
}

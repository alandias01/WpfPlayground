using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfPlayground.DesignPatterns.Unorganized
{
    /* Dictionary of templates keyed by template names
     * Each template maps to a dictionary of UI Controls->config strategies (IControlConfig)
     * You're encapsulating the logic for how a control should be configured inside an 
     * IControlConfig implementation (e.g., CheckboxControlConfig).
     * Then, ConfigManager is responsible for applying the right configuration for a given template.
     * 
     * Strategy pattern
     * Each IControlConfig is a strategy for configuring a control.
     * Example: CheckboxControlConfig encapsulates how a CheckBox should be initialized.
     * 
     * ABSFac
     * Your _templateConfigs dictionary is acting like a registry of factories.
     * Each template defines a set of control–config pairs, which is similar to 
     * how an Abstract Factory produces families of related objects (in this case, control configurations).
     * 
     * Registry / Lookup Table Pattern
     * The dictionary-of-dictionaries is essentially a registry that maps template names → control configs.
     * This is a common pragmatic pattern in C# when you want quick lookups without hardcoding logic.
     */

    public class ConfigManager
    {
        TextBox txt = new TextBox();
        CheckBox chk = new CheckBox();
        ComboBox cb = new ComboBox();
        Dictionary<string, Dictionary<Control, IControlConfig>> _templateConfigs;

        public ConfigManager()
        {

        }

        private void SetupTemplateConfig()
        {
            _templateConfigs = new Dictionary<string, Dictionary<Control, IControlConfig>>();
            _templateConfigs.Add("template1",
                new Dictionary<Control, IControlConfig>()
                {
                    {txt, new CheckboxControlConfig(chk, t => {
                                                                t.IsChecked = true;
                                                                t.IsEnabled = true;

                                                              })
                    }
                });
        }

        void RegisterTemplate(string key, Dictionary<Control, IControlConfig> template) { }

        public void SetDefaultControlSettings(string template)
        {
            if (_templateConfigs.TryGetValue(template, out var templateFound))
            {
            }
        }
    }


    public interface IControlConfig
    {
        void SetDefaults();
    }


    public class CheckboxControlConfig : IControlConfig
    {
        private Action<CheckBox> _setDefault;
        private CheckBox chk = new CheckBox();

        public CheckboxControlConfig(CheckBox control, Action<CheckBox> setDefault)
        {
            _setDefault = setDefault;
        }

        public void SetDefaults()
        {
        }
    }
}

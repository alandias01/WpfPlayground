using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPlayground.TriggersConverters
{
    /// <summary>
    /// Interaction logic for ValidationRule01.xaml
    /// </summary>
    public partial class ValidationRule01 : Window
    {
        public ValidationRule01()
        {
            InitializeComponent();
            this.DataContext = new ValidationRule01VM();
        }
    }

    /// <summary>
    /// View Model for the class
    /// </summary>
    public class ValidationRule01VM : INotifyPropertyChanged, IDataErrorInfo
    {
        private string userNameValidation01;
        private string userNameValidation02;
        private string userNameValidation03;
        private string userNameValidation04;

        public string UserNameValidation01
        {
            get => userNameValidation01;
            set
            {
                if (Regex.IsMatch(value, "[^a-zA-Z ]"))
                {
                    //throw new ArgumentException("Must be a letter");
                }
                else
                {
                    userNameValidation01 = value;
                }
                OnPropertyChanged();
            }
        }

        public string UserNameValidation02
        {
            get => userNameValidation02;
            set
            {
                if (Regex.IsMatch(value, "[^a-zA-Z ]"))
                    throw new ArgumentException("Must be a letter");

                userNameValidation02 = value;
                OnPropertyChanged();
            }
        }

        public string UserNameValidation03
        {
            get => userNameValidation03;
            set
            {
                userNameValidation03 = value;
                OnPropertyChanged();
            }
        }

        public string UserNameValidation04
        {
            get => userNameValidation04;
            set
            {
                userNameValidation04 = value;
                OnPropertyChanged();
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string name]
        {
            get
            {
                string result = null;
                if (name == "UserNameValidation03")
                {
                    if (!string.IsNullOrEmpty(userNameValidation03) && Regex.IsMatch(userNameValidation03, "[^a-zA-Z ]"))
                    {
                        result = "Must be a letter";
                    }
                }
                return result;
            }
        }

        public ValidationRule01VM()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class CharacterRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (Regex.IsMatch((string)value, "[^a-zA-Z ]"))
                {
                    return new ValidationResult(false, $"Illegal characters");
                }
                else
                {
                    return ValidationResult.ValidResult;
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Illegal characters or {e.Message}");
            }
        }
    }
}

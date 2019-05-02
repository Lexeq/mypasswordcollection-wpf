using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class InputWindowViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        private string _password;
        private string _passwordConfirmation;
        private ICommand _okCommand;
        private bool justShown;
        private readonly Dictionary<string, List<string>> errors;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors
        {
            get => errors.Values.FirstOrDefault(l => l.Count > 0) != null;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            errors.TryGetValue(propertyName, out List<string> errorsForProperty);
            return errorsForProperty.ToArray();
        }

        public bool DialogReult { get; private set; }

        public bool PasswordConfirmationRequired { get; }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                ValidateInput();
                OnPropertyChanged();
            }
        }

        public string PasswordConfirmation
        {
            get => _passwordConfirmation;
            set
            {
                _passwordConfirmation = value;
                ValidateInput();
                OnPropertyChanged();
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??
                  (_okCommand = new Command(() =>
                  {
                      DialogReult = true;
                  }, ()=> !justShown && !HasErrors));
            }
            set { _okCommand = value; }
        }

        public string Caption { get; set; }

        private void ValidateInput()
        {
            justShown = false;
            //check password
            var passwordErrors = errors[nameof(Password)];
            passwordErrors.Clear();
            if (string.IsNullOrEmpty(Password))
                passwordErrors.Add("Password can't be empty.");
            OnErrorsChanged(nameof(Password));

            //check password confirmation
            if (PasswordConfirmationRequired)
            {
                var confirmationErrors = errors[nameof(PasswordConfirmation)];
                confirmationErrors.Clear();
                if (PasswordConfirmation != Password)
                    confirmationErrors.Add("Passwords are not same.");
                OnErrorsChanged(nameof(PasswordConfirmation));
            }
         //   OnPropertyChanged(nameof(HasErrors));
        }

        private void OnErrorsChanged(string propName)
        {
            var handler = ErrorsChanged;
            handler?.Invoke(this, new DataErrorsChangedEventArgs(propName));
        }
        public InputWindowViewModel(bool passwordConfirmationRequired)
        {
            PasswordConfirmationRequired = passwordConfirmationRequired;
            Caption = string.Empty;
            _password = _passwordConfirmation = string.Empty; //fields shouldn't be null (in validation null == "" causes error for PasswordConfirmation)
            errors = new Dictionary<string, List<string>>()
            {
                {nameof(Password), new List<string>() },
                {nameof(PasswordConfirmation), new List<string>() }
            };
            justShown = true;

            OnPropertyChanged(nameof(HasErrors));
        }
    }
}
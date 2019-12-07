using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class InputWindowViewModel : DataErrorNotifyingViewModel
    {
        private string _password;
        private string _passwordConfirmation;
        private ICommand _okCommand;
        private bool _dialogResult;

        public event EventHandler ValidationPassed;

        public bool DialogReult
        {
            get => _dialogResult;
            private set
            {
                _dialogResult = value;
                OnPropertyChanged();
            }
        }

        public bool PasswordConfirmationRequired { get; }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string PasswordConfirmation
        {
            get => _passwordConfirmation;
            set
            {
                _passwordConfirmation = value;
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
                      ValidateInput();
                      if (!HasErrors)
                      {
                          DialogReult = true;
                          ValidationPassed?.Invoke(this, EventArgs.Empty);
                      }
                  }));
            }
            set { _okCommand = value; }
        }

        public string Caption { get; set; }

        private void ValidateInput()
        {
            ClearAllErrors();
            //check password
            if (string.IsNullOrEmpty(Password))
                AddPropertyError("Password can't be empty.", nameof(Password));

            //check password confirmation
            if (PasswordConfirmationRequired)
            {
                if (PasswordConfirmation != Password)
                    AddPropertyError("Passwords are not same.", nameof(PasswordConfirmation));
            }
        }

        public InputWindowViewModel(bool passwordConfirmationRequired)
        {
            PasswordConfirmationRequired = passwordConfirmationRequired;
            Caption = string.Empty;
            _password = _passwordConfirmation = string.Empty; //fields shouldn't be null (in validation null == "" causes error for PasswordConfirmation)
            OnPropertyChanged(nameof(HasErrors));
        }
    }
}
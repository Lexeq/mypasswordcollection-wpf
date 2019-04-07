using System;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class InputWindowViewModel : BaseViewModel
    {
        private bool isCorrect;
        private string _password;
        private string _passwordConfirmation;
        private ICommand _okCommand;

        private bool IsInputCorrect
        {
            get => isCorrect; set
            {
                isCorrect = value;
                OnPropertyChanged();
            }
        }

        public bool DialogReult { get; private set; }

        public bool PasswordConfirmationRequired { get; }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                var isValid = ValidatePassword();
                SetIsInputCorrect();
                if (!isValid)
                    throw new ArgumentException("Password can't be empty.");
                OnPropertyChanged();
            }
        }

        public string PasswordConfirmation
        {
            get => _passwordConfirmation;
            set
            {
                _passwordConfirmation = value;
                var isValid = ValidateConfirmation();
                SetIsInputCorrect();
                if (!isValid)
                    throw new ArgumentException("Passwords are not same.");
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
                  }, () => IsInputCorrect));
            }
            set { _okCommand = value; }
        }

        public string Caption { get; set; }

        private bool ValidatePassword()
        {
            return !string.IsNullOrEmpty(Password);
        }

        private bool ValidateConfirmation()
        {
            return Password == PasswordConfirmation;
        }

        private void SetIsInputCorrect()
        {
            IsInputCorrect = ValidatePassword() && (!PasswordConfirmationRequired || ValidateConfirmation());
        }

        public InputWindowViewModel(bool passwordConfirmationRequired)
        {
            PasswordConfirmationRequired = passwordConfirmationRequired;
            DialogReult = false;
        }
    }
}
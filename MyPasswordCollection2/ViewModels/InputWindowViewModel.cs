using System;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class InputWindowViewModel : BaseViewModel
    {
        public string Text { get; set; }

        public bool DialogReult { get; private set; }

        public bool PasswordConfirmationRequired { get; }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
              //  Validate();
            }
        }

        public string PasswordConfirmation
        {
            get => _passwordConfirmation;
            set
            {
                _passwordConfirmation = value;
                OnPropertyChanged();
                Validate();
            }
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(Password))
                throw new ApplicationException();
            if (PasswordConfirmationRequired)
            {
                if (PasswordConfirmation != Password)
                    throw new NotImplementedException();
            }
            return true;
        }

        private ICommand _okCommand;
        private string _passwordConfirmation;
        private string _password;

        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??
                  (_okCommand = new Command(() =>
                  {
                      DialogReult = true;
                  }, ()=> !string.IsNullOrEmpty(Password)));
            }
            set { _okCommand = value; }
        }

        public InputWindowViewModel(bool passwordConfirmationRequired)
        {
            PasswordConfirmationRequired = passwordConfirmationRequired;
            DialogReult = false;
        }
    }
}

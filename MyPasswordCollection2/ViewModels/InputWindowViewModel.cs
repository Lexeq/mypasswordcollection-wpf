using System;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class InputWindowViewModel : DataErrorNotifyingViewModel
    {
        IStringsSource uiStrings;
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
            get => _password ?? string.Empty;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string PasswordConfirmation
        {
            get => _passwordConfirmation ?? string.Empty;
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
                AddPropertyError(uiStrings.GetString(UIStrings.EmptyPasswordError), nameof(Password));

            //check password confirmation
            if (PasswordConfirmationRequired)
            {
                if (PasswordConfirmation != Password)
                    AddPropertyError(uiStrings.GetString(UIStrings.PasswordsAreNotSameError), nameof(PasswordConfirmation));
            }
        }

        public InputWindowViewModel(bool passwordConfirmationRequired, IStringsSource stringsSource)
        {
            uiStrings = stringsSource ?? throw new ArgumentNullException(nameof(stringsSource));
            PasswordConfirmationRequired = passwordConfirmationRequired;
            Caption = string.Empty;
            OnPropertyChanged(nameof(HasErrors));
        }
    }
}
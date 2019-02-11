using System.Windows.Input;

namespace MPC.ViewModels
{
    class InputWindowViewModel : BaseViewModel
    {
        public string Text { get; set; }

        public bool DialogReult { get; private set; }

        public bool PasswordConfirmationRequired { get; }

        public string Password { get; private set; }

        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get
            {
                return _okCommand ??
                  (_okCommand = new Command<string>((x) =>
                  {
                      DialogReult = true;
                      Password = x;
                  }));
            }
            set { _okCommand = value; }
        }

        public InputWindowViewModel(bool passwordConfirmationRequired)
        {
            PasswordConfirmationRequired = passwordConfirmationRequired;
        }
    }
}

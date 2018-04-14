using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class InputWindowViewModel : BaseViewModel
    {
        public string Title { get; set; }

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
            Title = "Enter password"; 
        }
    }
}

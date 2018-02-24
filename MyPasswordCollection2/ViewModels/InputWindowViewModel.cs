namespace MPC.ViewModels
{
    class InputWindowViewModel
    {
        public string Password { get; set; }

        public string PasswordConfirmation { get; set; }

        private Command _okCommand;
        public Command OkCommand
        {
            get
            {
                return _okCommand ??
                  (_okCommand = new Command(Ok, CheckPaswords));
            }
            set { _okCommand = value; }
        }

        private void Ok()
        {

        }

        private bool CheckPaswords()
        {
            return !string.IsNullOrEmpty(Password) &&
                   !string.IsNullOrEmpty(PasswordConfirmation) &&
                   Password == PasswordConfirmation;
        }
    }
}

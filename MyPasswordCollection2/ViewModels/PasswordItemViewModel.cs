using MPC.Model;
using System;
using System.Windows;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class PasswordItemViewModel : BaseViewModel
    {
        public PasswordItem Item { get; private set; }

        public string Site { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public PasswordItemViewModel(PasswordItem item)
        {
            this.Item = item ?? throw new ArgumentNullException(nameof(item));
            SetProperties();
        }

        private ICommand _copyCommand;
        public ICommand CopyCommand
        {
            get
            {
                return _copyCommand ??
                  (_copyCommand = new Command<string>(CopyToClipboard, (s)=> Item != null));
            }
            set { _copyCommand = value; }
        }

        public void DeclineChanges()
        {
            SetProperties();
        }

        public void AcceptChanges()
        {
            Item.Site = Site;
            Item.Login = Login;
            Item.Password = Password;
            RaisePropertyChangedEvent();
        }

        private void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }

        private void SetProperties()
        {
            Site = Item.Site;
            Login = Item.Login;
            Password = Item.Password;
            RaisePropertyChangedEvent();
        }

        private void RaisePropertyChangedEvent()
        {
            OnPropertyChanged(nameof(Site));
            OnPropertyChanged(nameof(Login));
            OnPropertyChanged(nameof(Password));
        }
    }
}

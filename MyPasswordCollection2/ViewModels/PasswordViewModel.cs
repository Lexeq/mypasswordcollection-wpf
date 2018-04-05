using PasswordStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class PasswordViewModel : BaseViewModel
    {
        private bool _editMode;
        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                OnPropertyChanged(nameof(EditMode));
            }
        }

        

        private ICommand _copyCommand;

        public ICommand CopyCommand
        {
            get {
                return _copyCommand ??
                  (_copyCommand = new Command<string>(CopyToClipboard)); }
            set { _copyCommand = value; }
        }



        public PasswordItem PasswordItem { get; private set; }

        public PasswordViewModel(PasswordItem passwordItem)
        {
            this.PasswordItem = passwordItem;
        }

        public void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }
    }
}

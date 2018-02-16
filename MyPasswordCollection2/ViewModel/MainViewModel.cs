using PasswordStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MPC.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private IMessageService messageService;

        private IIOService ioService;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _searchString;
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                OnPropertyChanged(nameof(SearchString));
            }
        }

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

        private PasswordItem _selectedItem;
        public PasswordItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private PasswordSource _passwords;
        public PasswordSource Passwords
        {
            get { return _passwords; }
            set
            {
                _passwords = value;
                OnPropertyChanged(nameof(Passwords));
            }
        }


        #region Commands
        private Command _addCommand;
        public Command AddCommand
        {
            get
            {
                return _addCommand ??
                    (_addCommand = new Command(AddPassword, () => SelectedItem != null));
            }
            set { _addCommand = value; }
        }

        private Command _removeCommand;
        public Command RemoveCommand
        {
            get
            {
                return _removeCommand ??
                  (_removeCommand = new Command(RemovePassword, () => SelectedItem != null));

            }
            set { _removeCommand = value; }
        }

        private Command _openFileCommand;
        public Command OpenFileCommand
        {
            get
            {
                return _openFileCommand ??
                    (_openFileCommand = new Command(OpenFile));
            }
            set { _openFileCommand = value; }
        }


        //!!!
        private Command _newPasswordCollection;

        public Command NewPasswordCollection
        {
            get { return _newPasswordCollection; }
            set { _newPasswordCollection = value; }
        }





        #endregion

        public MainWindowViewModel(IMessageService mService, IIOService ioService)
        {
            messageService = mService;
            this.ioService = ioService;

            OpenFileCommand = new Command(OpenFile);
        }

        protected void OnPropertyChanged(string propName)
        {
            var t = PropertyChanged;
            t?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void AddPassword()
        {

        }

        private void RemovePassword()
        {

        }

        private void OpenFile()
        {
            var path = ioService.OpenFileDialog(Application.Current.StartupUri.ToString());
            Passwords = new PasswordSource(path, );
        }
    }
}

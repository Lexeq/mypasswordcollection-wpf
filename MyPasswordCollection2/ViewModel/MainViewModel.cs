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
        private Command addCommand;

        public Command AddCommand
        {
            get { return addCommand; }
            set { addCommand = value; }
        }


        private Command _removeCommand;

        public Command RemoveCommand
        {
            get { return _removeCommand; }
            set { _removeCommand = value; }
        }


        private Command _newPasswordCollection;

        public Command NewPasswordCollection
        {
            get { return _newPasswordCollection; }
            set { _newPasswordCollection = value; }
        }


        #endregion

        public MainWindowViewModel()
        {
            SearchString = "i'm here";

            AddCommand = new Command(AddPassword, (o) => SelectedItem != null);
            RemoveCommand = new Command(RemovePassword, (o) => SelectedItem != null);
        }

        protected void OnPropertyChanged(string propName)
        {
            var t = PropertyChanged;
            t?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void AddPassword(object param)
        {

        }

        private void RemovePassword(object param)
        {

        }
    }
}

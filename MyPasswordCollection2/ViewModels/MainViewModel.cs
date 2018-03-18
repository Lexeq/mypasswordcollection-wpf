using PasswordStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MPC.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private IDialogService dialogs;

        private IWindowsManager windows;

        #region Properties
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

        private PasswordSource _passwordSrc;
        public PasswordSource PasswordSource
        {
            get { return _passwordSrc; }
            set
            {
                _passwordSrc = value;
                OnPropertyChanged(nameof(PasswordSource));
            }
        }
        #endregion

        #region Commands
        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get
            {
                return _addCommand ??
                    (_addCommand = new Command(AddPassword, ()=> PasswordSource != null && !EditMode));
            }
            set { _addCommand = value; }
        }

        private ICommand _removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand ??
                  (_removeCommand = new Command<PasswordItem>(RemovePassword, (o) => SelectedItem != null && SelectedItem is PasswordItem));
            }
            set { _removeCommand = value; }
        }

        private ICommand _openFileCommand;
        public ICommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ??
                    (_openFileCommand = new Command(OpenFile));
            }
            set { _openFileCommand = value; }
        }

        private ICommand _cancelAddingCommand;
        public ICommand CancelAddingCommand
        {
            get {
                return _cancelAddingCommand ??
                  (_cancelAddingCommand = new Command(CancelAdding)); }
            set { _cancelAddingCommand = value; }
        }

        private ICommand _finishlAddingCommand;
        public ICommand FinishAddingCommand
        {
            get { return _finishlAddingCommand ??
                    (_finishlAddingCommand = new Command(FinishAdding)); }
            set { _finishlAddingCommand = value; }
        }

        private ICommand _newPasswordCollectionCommand;
        public ICommand NewPasswordCollectionCommand
        {
            get
            {
                return _newPasswordCollectionCommand ??
                    (_newPasswordCollectionCommand = new Command(NewFile));
            }
            set { _newPasswordCollectionCommand = value; }
        }
        #endregion

        public MainWindowViewModel(IDialogService dialogService, IWindowsManager winManager)
        {
            dialogs = dialogService;
            windows = winManager;
            EditMode = false;
        }

        private void AddPassword()
        {
            SelectedItem = new PasswordItem();
            EditMode = true;
        }

        private void RemovePassword(PasswordItem item)
        {
            PasswordSource.Passwords.Remove(item);
        }

        private void NewFile()
        {
            var settings = new FileDialogSettings
            {
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                Filter = "Password file(*.pw) | *.pw"
            };

            var result = dialogs.ShowSaveFileDialog(settings);
            if(result)
            {
                InputWindowViewModel inputVM = new InputWindowViewModel(true);
                windows.ShowDialog(inputVM);
                if (inputVM.DialogReult)
                {
                    try
                    {
                        if (File.Exists(settings.FileName))
                            File.Delete(settings.FileName);

                        PasswordSource = new PasswordSource(settings.FileName, inputVM.Password);
                    }
                    catch(Exception e)
                    {
                        dialogs.ShowMessage($"Failed to initialize password collection:\n{e.Message}", "Error");
                    }
                }
            }
        }

        private void OpenFile()
        {
            var settings = new FileDialogSettings
            {
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                Filter = "Password file(*.pw) | *.pw"
            };

            var result = dialogs.ShowOpenFileDialog(settings);
            if (result)
            {
                InputWindowViewModel inputVM = new InputWindowViewModel(false);
                windows.ShowDialog(inputVM);
                if (inputVM.DialogReult)
                {
                    try
                    {
                        PasswordSource = new PasswordSource(settings.FileName, inputVM.Password);
                    }
                    catch (Exception e)
                    {
                        dialogs.ShowMessage($"Failed to load password collection:\n{e.Message}", "Error");
                    }
                }
            }
        }

        private void CancelAdding()
        {
            EditMode = false;
            SelectedItem = null;
        }

        private void FinishAdding()
        {
            PasswordSource.Passwords.Add(SelectedItem);
            EditMode = false;
        }
    }
}

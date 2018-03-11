using PasswordStorage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        #endregion

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
        private Command _newPasswordCollectionCommand;

        public Command NewPasswordCollectionCommand
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
        }

        private void AddPassword()
        {

        }

        private void RemovePassword()
        {

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

                        Passwords = new PasswordSource(settings.FileName, inputVM.Password);
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
                        Passwords = new PasswordSource(settings.FileName, inputVM.Password);
                    }
                    catch (Exception e)
                    {
                        dialogs.ShowMessage($"Failed to load password collection:\n{e.Message}", "Error");
                    }
                }
            }
        }
    }
}

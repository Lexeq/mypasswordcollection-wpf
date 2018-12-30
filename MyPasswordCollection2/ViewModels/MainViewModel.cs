using MPC.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Input;
using System.Linq;

namespace MPC.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private SearchHelper searchHelper;

        private IDialogService dialogs;

        private IWindowsManager windows;

        private IRepositoryManager repoManager;
        #endregion

        #region Properties
        private string _searchString;
        public string SearchString
        {
            get { return _searchString ?? ""; }
            set
            {
                _searchString = value;
                if (searchHelper != null)
                {
                    searchHelper.SearchString = value;
                    FindNext();
                }

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

        private PasswordItemViewModel _selectedItem;
        public PasswordItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (EditMode)
                    CancelEdit();
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private IPasswordRepository _passwordSrc;
        public IPasswordRepository PasswordSource
        {
            get { return _passwordSrc; }
            set
            {
                _passwordSrc = value;
                if (_passwordSrc != null)
                {
                    searchHelper = new SearchHelper(_passwordSrc) { AutoReset = true, SearchString = SearchString };
                    Items = new ObservableCollection<PasswordItemViewModel>(value?.Select(x => new PasswordItemViewModel(x)));
                }
                else
                {
                    searchHelper = null;
                    Items = null;
                }
                OnPropertyChanged(nameof(PasswordSource));
            }
        }

        public ObservableCollection<PasswordItemViewModel> items;
        public ObservableCollection<PasswordItemViewModel> Items
        {
            get => items; set
            {
                items = value;
                OnPropertyChanged();
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
                    (_addCommand = new Command(AddPassword, () => PasswordSource != null && !EditMode));
            }
            set { _addCommand = value; }
        }

        private ICommand _removeCommand;
        public ICommand RemoveCommand
        {
            get
            {
                return _removeCommand ??
                  (_removeCommand = new Command<PasswordItemViewModel>(RemovePassword, (o) => SelectedItem != null));
            }
            set { _removeCommand = value; }
        }

        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                return _clearCommand ??
                   (_clearCommand = new Command(ClearCollection, () => PasswordSource != null));
            }
            set
            {
                _clearCommand = value;
            }
        }

        private ICommand _deleteCollectionCommand;
        public ICommand DeleteCollectionCommand
        {
            get
            {
                return _deleteCollectionCommand ??
                    (_deleteCollectionCommand = new Command(DeleteCollection, () => PasswordSource != null));
            }
            set
            {
                _deleteCollectionCommand = value;
            }
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
        public ICommand CancelEditCommand
        {
            get
            {
                return _cancelAddingCommand ??
                  (_cancelAddingCommand = new Command(CancelEdit));
            }
            set { _cancelAddingCommand = value; }
        }

        private ICommand _finishlAddingCommand;
        public ICommand FinishEditCommand
        {
            get
            {
                return _finishlAddingCommand ??
                  (_finishlAddingCommand = new Command(FinishEdit));
            }
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

        private ICommand _changePasswordCommand;
        public ICommand ChangePasswordCommand
        {
            get
            {
                return _changePasswordCommand ??
                  (_changePasswordCommand = new Command(ChangePassword, () => PasswordSource != null));
            }
            set { _changePasswordCommand = value; }
        }

        private ICommand _editCommand;
        public ICommand EditCommand
        {
            get
            {
                return _editCommand ??
                  (_editCommand = new Command(EditItem, () => SelectedItem != null));
            }
            set { _editCommand = value; }
        }

        private ICommand _findCommand;
        public ICommand FindCommand
        {
            get
            {
                return _findCommand ??
                    (_findCommand = new Command(FindNext, () => PasswordSource != null));
            }
            set { _findCommand = value; }
        }

        private ICommand _closeCommand;
        public ICommand CloseFileCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new Command(CloseFile, () => PasswordSource != null));
            }
        }

        private ICommand _showAboutCommand;

        public ICommand ShowAboutCommand
        {
            get
            {
                return _showAboutCommand ?? (_showAboutCommand = new Command(() => windows.ShowDialog(new AboutViewModel())));
            }
        }
        #endregion

        #region Ctor
        public MainWindowViewModel(IDialogService dialogService, IWindowsManager winManager, IRepositoryManager repoManager)
        {
            dialogs = dialogService;
            windows = winManager;
            this.repoManager = repoManager;
            EditMode = false;
            //  Pivm = new PasswordItemVewModel(null);
        }
        #endregion

        #region Methods
        private void AddPassword()
        {
            SelectedItem = new PasswordItemViewModel(new PasswordItem());
            EditMode = true;
        }

        private void RemovePassword(PasswordItemViewModel item)
        {
            if(PasswordSource.Remove(item.Item))
            {
                items.Remove(item);
            }
        }

        private void ClearCollection()
        {
            if (PasswordSource == null)
                throw new NullReferenceException(nameof(PasswordSource));

            if (dialogs.ShowDialog("All passwords will be removed. Continue?", "Warning"))
                PasswordSource.Clear();
        }

        private void DeleteCollection()
        {
            if (PasswordSource == null)
                throw new NullReferenceException(nameof(PasswordSource));

            //TODO: Implement

            //var path = PasswordSource.FilePath;
            //try
            //{
            //    File.Delete(path);
            //}
            //catch (Exception e)
            //{
            //    dialogs.ShowMessage($"Failed to remove file.\n [{e.Message}", "Error");
            //}
            PasswordSource = null;
            searchHelper = null;
        }

        private void ChangePassword()
        {
            if (PasswordSource == null)
                throw new NullReferenceException(nameof(PasswordSource));

            var oldPassInputVM = new InputWindowViewModel(false) { Title = "Enter old password" };
            windows.ShowDialog(oldPassInputVM);
            if (oldPassInputVM.DialogReult == false)
                return;
            var newPassInputVW = new InputWindowViewModel(true) { Title = "Enter new password" };
            windows.ShowDialog(newPassInputVW);
            if (newPassInputVW.DialogReult == false)
                return;
            try
            {
                //TODO: IMPLEMENT
                // PasswordSource.ChangePassword(oldPassInputVM.Password, newPassInputVW.Password);
            }
            catch (Exception e)
            {
                dialogs.ShowMessage($"Can't change password\n{e.Message}", "Failed");
            }
        }

        private void NewFile()
        {
            var settings = new FileDialogSettings
            {
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                Filter = "Password file(*.pw) | *.pw"
            };

            var result = dialogs.ShowSaveFileDialog(settings);
            if (result)
            {
                InputWindowViewModel inputVM = new InputWindowViewModel(true);
                windows.ShowDialog(inputVM);
                if (inputVM.DialogReult)
                {
                    try
                    {
                        if (File.Exists(settings.FileName))
                            File.Delete(settings.FileName);

                        PasswordSource = repoManager.GetRepository(settings.FileName, inputVM.Password);
                    }
                    catch (Exception e)
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
                bool retryFlag = false;
                do
                {
                    InputWindowViewModel inputVM = new InputWindowViewModel(false);
                    windows.ShowDialog(inputVM);
                    if (inputVM.DialogReult)
                    {
                        try
                        {
                            retryFlag = false;
                            PasswordSource = repoManager.GetRepository(settings.FileName, inputVM.Password);
                        }
                        catch (CryptographicException)
                        {
                            retryFlag = dialogs.ShowDialog("Decryption failed. Please check the password. Try again?", "");
                        }
                        catch (Exception e)
                        {
                            dialogs.ShowMessage($"Failed to load password collection:\n{e.Message}", "Error");
                        }
                    }
                } while (retryFlag);
            }
        }

        private void CloseFile()
        {
            PasswordSource.Dispose();
            PasswordSource = null;
        }

        private void EditItem()
        {
            EditMode = true;
        }

        private void CancelEdit()
        {
            SelectedItem.DeclineChanges();
            EditMode = false;
        }

        private void FinishEdit()
        {
            SelectedItem.AcceptChanges();
            PasswordSource.Save(SelectedItem.Item);
            if (!Items.Contains(SelectedItem))
                Items.Add(SelectedItem);
            EditMode = false;
        }

        private void FindNext()
        {
            if (PasswordSource != null && !string.IsNullOrEmpty(SearchString))
            {
                var index = searchHelper.FindNext();
                SelectedItem = index >= 0 ? Items[index] : null;
            }
        }

        #endregion
    }
}
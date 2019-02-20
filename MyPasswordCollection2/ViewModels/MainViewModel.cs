using MPC.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;
using System.Windows.Data;
using System.Globalization;

namespace MPC.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private IDialogService dialogs;

        private IWindowsManager windows;

        private IRepositoryManager repoManager;

        #endregion

        #region Properties
        private string _filterString;
        public string FilterString
        {
            get { return _filterString ?? ""; }
            set
            {
                _filterString = value;
                if (CollectionView != null)
                {
                    using (CollectionView.DeferRefresh())
                    {
                        var filter = new FilterHelper(value);
                        CollectionView.CustomSort = filter;
                        CollectionView.Filter = filter.Filter;
                    }
                    CollectionView.MoveCurrentToFirst();

                }
                OnPropertyChanged(nameof(FilterString));
            }
        }

        private ListCollectionView lcv;
        public ListCollectionView CollectionView
        {
            get => lcv;
            set
            {
                lcv = value;
                OnPropertyChanged();
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
                _passwordSrc?.Dispose();
                _passwordSrc = value;
                FilterString = string.Empty;
                if (_passwordSrc != null)
                {
                    Items = new ObservableCollection<PasswordItemViewModel>(value?.Select(x => new PasswordItemViewModel(x)));
                    CollectionView = new ListCollectionView(Items);
                }
                else
                {
                    Items = null;
                    CollectionView = null;
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

        #region AddCommand
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


        #endregion

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
                   (_clearCommand = new Command(ClearRepository, () => PasswordSource != null));
            }
            set
            {
                _clearCommand = value;
            }
        }

        private ICommand _deleteRepositoryCommand;
        public ICommand DeleteRepositoryCommand
        {
            get
            {
                return _deleteRepositoryCommand ??
                    (_deleteRepositoryCommand = new Command(DeleteRepository, () => PasswordSource != null));
            }
            set
            {
                _deleteRepositoryCommand = value;
            }
        }

        private ICommand _openRepositoryCommand;
        public ICommand OpenRepositoryCommand
        {
            get
            {
                return _openRepositoryCommand ??
                    (_openRepositoryCommand = new Command(OpenRepository));
            }
            set { _openRepositoryCommand = value; }
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

        private ICommand _newRepositoryCommand;
        public ICommand NewRepositoryCommand
        {
            get
            {
                return _newRepositoryCommand ??
                    (_newRepositoryCommand = new Command(NewRepository));
            }
            set { _newRepositoryCommand = value; }
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

        private ICommand _closeCommand;
        public ICommand CloseRepositoryCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new Command(CloseRepository, () => PasswordSource != null));
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

        private ICommand _clearFilterCommand;
        public ICommand ClearFilterCommand
        {
            get
            {
                return _clearFilterCommand ?? (_clearFilterCommand = new Command(() => FilterString = "", () => CollectionView != null));
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
            try
            {
                if (PasswordSource.Remove(item.Item))
                {
                    items.Remove(item);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ClearRepository()
        {
            try
            {
                if (dialogs.ShowDialog("All passwords will be removed. Continue?", "Warning"))
                {
                    PasswordSource.Clear();
                    items.Clear();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void DeleteRepository()
        {
            try
            {
                repoManager.DeleteRepository(PasswordSource);
                PasswordSource = null;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ChangePassword()
        {
            var oldPassInputVM = new InputWindowViewModel(false) { Text = "Enter old password" };
            windows.ShowDialog(oldPassInputVM);
            if (oldPassInputVM.DialogReult == false)
                return;
            var newPassInputVW = new InputWindowViewModel(true) { Text = "Enter new password" };
            windows.ShowDialog(newPassInputVW);
            if (newPassInputVW.DialogReult == false)
                return;
            try
            {
                PasswordSource.ChangePassword(oldPassInputVM.Password, newPassInputVW.Password);
            }
            catch (PasswordException)
            {
                dialogs.ShowMessage($"Can't change password.", "Failed");
            }
        }

        private void NewRepository()
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
                        PasswordSource = repoManager.Create(settings.FileName, inputVM.Password);
                    }
                    catch (Exception e)
                    {
                        HandleException(e);
                    }
                }
            }
        }

        private void OpenRepository()
        {
            var settings = new FileDialogSettings
            {
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                Filter = "Password file(*.pw) | *.pw"
            };

            var result = dialogs.ShowOpenFileDialog(settings);
            if (result)
            {
                while (true)
                {
                    InputWindowViewModel inputVM = new InputWindowViewModel(false);
                    windows.ShowDialog(inputVM);
                    if (inputVM.DialogReult)
                    {
                        try
                        {
                            PasswordSource = repoManager.Get(settings.FileName, inputVM.Password);
                            return;
                        }
                        catch (PasswordException)
                        {
                            if (!dialogs.ShowDialog("Please check the password. Try again?", "Decryption failed"))
                                return;
                        }
                        catch (Exception e)
                        {
                            PasswordSource = null;
                            HandleException(e);
                            return;
                        }
                    }
                    else
                        return;
                }
            }
        }

        private void CloseRepository()
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

        private void HandleException(Exception ex)
        {
            windows.ShowDialog(new ExceptionViewModel(ex));
        }

        #endregion
    }
}
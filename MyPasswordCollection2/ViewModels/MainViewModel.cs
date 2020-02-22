using MPC.Model;
using System;
using System.Windows.Input;
using System.Linq;
using System.Windows.Data;
using System.Collections.Generic;
using System.IO;

namespace MPC.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private readonly IDialogService dialogs;

        private readonly IWindowsManager windows;

        private readonly IRepositoryManager repoManager;

        private FilterHelper filterHelper;

        private IStringsSource uiStrings;
        #endregion

        #region Properties
        private string _filterString;
        public string FilterString
        {
            get { return _filterString ?? ""; }
            set
            {
                _filterString = value;
                if (filterHelper != null)
                {
                    filterHelper.FilterString = value;
                }
                if (EditMode)
                {
                    CancelEdit();
                }
                CollectionView?.Refresh();
                CollectionView?.MoveCurrentToFirst();
                OnPropertyChanged(nameof(FilterString));
            }
        }

        private ListCollectionView _collectionView;
        public ListCollectionView CollectionView
        {
            get => _collectionView;
            set
            {
                _collectionView = value;
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

        private PasswordItem _selectedItem;
        public PasswordItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (EditMode)
                    EditMode = false;
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
                FilterString = string.Empty;

                if (value != null)
                {
                    var repoProxy = new PasswordRepositoryProxy(value);
                    filterHelper = new FilterHelper(repoProxy);
                    CollectionView = new ListCollectionView(repoProxy)
                    {
                        CustomSort = filterHelper,
                        Filter = filterHelper.PassesFilter
                    };
                    _passwordSrc = repoProxy;
                }
                else
                {
                    _passwordSrc = null;
                    filterHelper = null;
                    CollectionView = null;
                }

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
                  (_removeCommand = new Command(() => RemovePassword(SelectedItem), () => SelectedItem != null && !EditMode));
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

        private ICommand _createRepositoryCommand;
        public ICommand CreateRepositoryCommand
        {
            get
            {
                return _createRepositoryCommand ??
                    (_createRepositoryCommand = new Command(CreateRepository));
            }
            set { _createRepositoryCommand = value; }
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
                return _showAboutCommand ?? (_showAboutCommand = new Command(() => windows.ShowDialog(new AboutViewModel(uiStrings.GetString(UIStrings.AboutText)))));
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

        private ICommand passwordGenerationCommand;
        public ICommand PasswordGenerationCommand
        {
            get => passwordGenerationCommand ?? (passwordGenerationCommand = new Command(() => windows.ShowDialog<PasswordGenerationViewModel>(new PasswordGenerationViewModel())));
        }
        #endregion

        #region Ctor
        public MainWindowViewModel(IDialogService dialogService, IWindowsManager winManager, IRepositoryManager repoManager, IStringsSource stringsSource)
        {
            dialogs = dialogService;
            windows = winManager;
            this.repoManager = repoManager;
            uiStrings = stringsSource;
            EditMode = false;
        }
        #endregion

        #region Methods
        private void AddPassword()
        {
            SelectedItem = new PasswordItem();
            EditMode = true;
        }

        private void RemovePassword(PasswordItem item)
        {
            if (!dialogs.ShowDialog(uiStrings.GetString(UIStrings.DeleteConfirmation, new Dictionary<string, string>() { [nameof(item.Site)] = item.Site }), uiStrings.GetString(UIStrings.ConfirmationWindowTitle)))
                return;
            try
            {
                PasswordSource.Remove(item);
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
                if (dialogs.ShowDialog(uiStrings.GetString(UIStrings.ClearConfirmation), uiStrings.GetString(UIStrings.ConfirmationWindowTitle)))
                {
                    PasswordSource.Clear();
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
            var oldPassInputVM = new InputWindowViewModel(false, uiStrings) { Caption = uiStrings.GetString(UIStrings.OldPasswordRequest) };
            windows.ShowDialog(oldPassInputVM);
            if (oldPassInputVM.DialogReult == false)
                return;
            var newPassInputVW = new InputWindowViewModel(true, uiStrings) { Caption = uiStrings.GetString(UIStrings.NewPasswordRequest) };
            windows.ShowDialog(newPassInputVW);
            if (newPassInputVW.DialogReult == false)
                return;
            try
            {
                PasswordSource.ChangePassword(oldPassInputVM.Password, newPassInputVW.Password);
            }
            catch (PasswordException)
            {
                dialogs.ShowMessage(uiStrings.GetString(UIStrings.ChangePasswordFail), uiStrings.GetString(UIStrings.Failed));
            }
        }

        private void CreateRepository()
        {
            InputWindowViewModel passwordInput = new InputWindowViewModel(true, uiStrings);
            windows.ShowDialog(passwordInput);
            if (passwordInput.DialogReult)
            {
                var result = dialogs.ShowSaveDialog(out string path);
                if (result)
                {

                    try
                    {
                        PasswordSource = repoManager.Create(path, passwordInput.Password);
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
            var result = dialogs.ShowOpenDialog(out string path);
            if (result)
            {
                OpenRepository(path);
            }
        }

        private void OpenRepository(string path)
        {
            while (true)
            {
                InputWindowViewModel passwordInput = new InputWindowViewModel(false, uiStrings);
                windows.ShowDialog(passwordInput);
                if (passwordInput.DialogReult)
                {
                    try
                    {
                        PasswordSource = repoManager.Get(path, passwordInput.Password);
                        return;
                    }
                    catch (PasswordException)
                    {
                        if (dialogs.ShowDialog(uiStrings.GetString(UIStrings.RetryPasswordEnter), uiStrings.GetString(UIStrings.DecryptionFail)))
                            continue;
                    }
                    catch(RepositoryException e) when (e.InnerException is IOException)
                    {
                        dialogs.ShowMessage("Repository already in use or no access.", "Loading failed");
                    }
                    catch(RepositoryException)
                    {
                        dialogs.ShowMessage("Repository corrupted.", "Loading failed");
                    }
                    catch (Exception e)
                    {
                        PasswordSource = null;
                        HandleException(e);
                    }
                    return;
                }
                else
                {
                    return;
                }
            }
        }

        private void CloseRepository()
        {
            PasswordSource = null;
        }

        private void EditItem()
        {
            EditMode = true;
        }

        private void CancelEdit()
        {
            EditMode = false;
            SelectedItem = (PasswordItem)CollectionView.CurrentItem;
        }

        private void FinishEdit()
        {
            EditMode = false;
            try
            {
                PasswordSource.Save(SelectedItem);
            }
            catch (Exception e)
            {
                HandleException(e);
            }
            SelectedItem = (PasswordItem)CollectionView.CurrentItem;
        }

        private void HandleException(Exception ex)
        {
            windows.ShowDialog(new ExceptionViewModel(ex));
        }

        #endregion
    }
}
using Microsoft.Win32;
using MPC.ViewModels;
using MPC.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace MPC
{
    public class WindowsManager : IWindowsManager, IDialogService
    {
        private readonly IDictionary<string, Type> winByKey = new Dictionary<string, Type>();

        private readonly string Filter = "Password file(*.pw) | *.pw";

        private readonly string InitialDirectory = Directory.GetCurrentDirectory();

        #region IWindowsManager

        public void Show<TViewModel>(TViewModel vm)
        {
            Show(vm, false);
        }

        public void ShowDialog<TViewModel>(TViewModel vm)
        {
            Show(vm, true);
        }

        #endregion

        #region IDialogService
        public bool ShowMessageDialog(string message, string caption, DialogButtons buttons)
        {
            return DialogWindow.Show(message, caption, buttons, GetActiveWindow());
        }

        public bool ShowOpenDialog(out string path)
        {
            path = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = InitialDirectory,
                Filter = Filter
            };
            var dialogResult = ofd.ShowDialog();
            if (dialogResult == true)
            {
                path = ofd.FileName;
                return true;
            }
            else
                return false;
        }

        public bool ShowSaveDialog(out string path)
        {
            path = string.Empty;
            SaveFileDialog sfd = new SaveFileDialog
            {
                InitialDirectory = InitialDirectory,
                Filter = Filter
            };
            var dialogResult = sfd.ShowDialog();
            if (dialogResult == true)
            {
                path = sfd.FileName;
                return true;
            }
            else
                return false;
        }

        #endregion

        public void Register<TViewModel, TWindow>()
           where TWindow : Window
        {
            var key = typeof(TViewModel).FullName;
            if (winByKey.ContainsKey(key))
                throw new ArgumentException($"ViewModel {key} has already registered");

            winByKey[key] = typeof(TWindow);
        }

        private void Show<TViewModel>(TViewModel viewModel, bool dialogMode)
        {
            var key = typeof(TViewModel).FullName;
            if (!winByKey.ContainsKey(key))
                throw new ArgumentException($"Key {key} wasn't registered", nameof(key));
            var windowType = winByKey[key];
            var window = (Window)Activator.CreateInstance(windowType);
            window.DataContext = viewModel;
            window.Owner = GetActiveWindow();
            window.WindowStartupLocation = window.Owner == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
            if (dialogMode)
                window.ShowDialog();
            else
                window.Show();
        }

        private Window GetActiveWindow()
        {
            return Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
        }
    }
}
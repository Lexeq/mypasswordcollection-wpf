using MPC.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MPC
{
    public class WindowsManager : IWindowsManager
    {
        IDictionary<string, Type> winByKey = new Dictionary<string, Type>();

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

            if (dialogMode)
                window.ShowDialog();
            else
                window.Show();
        }

        public void Show<TViewModel>(TViewModel vm)
        {
            Show(vm, false);
        }

        public void ShowDialog<TViewModel>(TViewModel vm)
        {
            Show(vm, true);
        }
    }
}

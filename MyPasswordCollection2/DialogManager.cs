﻿using MPC.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MPC
{
    public class DialogManager : IDialogManager
    {
        private readonly IDictionary<string, FrameworkElement> _viewsByKey = new Dictionary<string, FrameworkElement>();

        public void Register<TView>(string key, TView view) where TView : FrameworkElement
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (view == null)
                throw new ArgumentNullException(nameof(view));
            if (_viewsByKey.ContainsKey(key))
                throw new ArgumentException($"Key {key} has already registered", nameof(key));

            _viewsByKey[key] = view;
        }

        public void ShowDialog(string key, object viewModel)
        {
            if (!_viewsByKey.ContainsKey(key))
                throw new ArgumentException($"Key {key} wasn't registered", nameof(key));

            var view = _viewsByKey[key];
            var window = new Window()
            {
                Content = view,
                DataContext = viewModel,
            };
            window.ShowDialog();
        }
    }

    public static class DialogManagerExtensions
    {
        public static void Register<TViewModel, TView>(this DialogManager dialogManager)
            where TView : FrameworkElement, new()
        {
            var key = typeof(TViewModel).FullName;
            dialogManager.Register(key, new TView());
        }
    }
}

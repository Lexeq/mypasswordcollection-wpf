﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MPC.Views
{
    public enum DialogButtons { OK, YesNo, OkCancel }

    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public sealed partial class DialogWindow : Window
    {
        private DialogWindow()
        {
            InitializeComponent();
        }

        private void SetButtons(DialogButtons buttons)
        {
            switch (buttons)
            {
                case DialogButtons.OK:
                    AddButton("OK", true);
                    break;
                case DialogButtons.YesNo:
                    AddButton("Yes", true);
                    AddButton("No", false);
                    break;
                case DialogButtons.OkCancel:
                    AddButton("OK", true);
                    AddButton("Cancel", false);
                    break;
                default:
                    throw new ArgumentException("Invalid value", nameof(buttons));
            }
        }

        private void AddButton(string text, bool result)
        {
            var button = new Button() { Content = text, IsDefault = result, IsCancel = !result };
            button.Click += (o, args) => { DialogResult = result; };
            buttonContainer.Children.Add(button);
        }

        public static bool Show(string message, string caption, DialogButtons buttons)
        {
            var dialog = new DialogWindow();
            dialog.tbMessage.Text = message;
            dialog.Title = caption;
            dialog.SetButtons(buttons);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.ShowDialog();
            return dialog.DialogResult.HasValue && dialog.DialogResult == true;
        }
    }
}
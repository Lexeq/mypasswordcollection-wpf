using MPC.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MPC.Views
{

    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public sealed partial class DialogWindow : Window
    {
        private DialogWindow(DialogButtons buttons)
        {
            InitializeComponent();
            SetButtons(buttons);
        }

        private void SetButtons(DialogButtons buttons)
        {
            switch (buttons)
            {
                case DialogButtons.OK:
                    AddButton("button.ok", true);
                    break;
                case DialogButtons.YesNo:
                    AddButton("button.yes", true);
                    AddButton("button.no", false);
                    break;
                case DialogButtons.OkCancel:
                    AddButton("button.ok", true);
                    AddButton("button.cancel", false);
                    break;
                default:
                    throw new ArgumentException("Invalid value", nameof(buttons));
            }
        }

        private void AddButton(string text, bool result)
        {
            var button = new Button() { IsDefault = result, IsCancel = !result };
            button.SetResourceReference(Button.ContentProperty, text);
            button.Click += (o, args) => { DialogResult = result; };
            buttonContainer.Children.Add(button);
            if (button.IsDefault)
            {
                button.Focus();
            }
        }

        public static bool Show(string message, string caption, DialogButtons buttons, Window owner)
        {
            var dialog = new DialogWindow(buttons);
            dialog.tbMessage.Text = message;
            dialog.Title = caption;
            dialog.Owner = owner;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dialog.ShowDialog();
            return dialog.DialogResult.HasValue && dialog.DialogResult == true;
        }
    }
}
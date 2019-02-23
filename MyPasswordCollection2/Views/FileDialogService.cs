using Microsoft.Win32;
using MPC.ViewModels;
using System.IO;
using System.Windows;

namespace MPC.Views
{
    class FileDialogService : IDialogService
    {
        private readonly string Filter = "Password file(*.pw) | *.pw";

        private readonly string InitialDirectory = Directory.GetCurrentDirectory();

        public void ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption);
        }

        public bool ShowDialog(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
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
    }
}

using Microsoft.Win32;
using MPC.ViewModels;
using System.Windows;

namespace MPC
{
    class DialogService : IDialogService
    {
        public void ShowMessage(string message, string caption)
        {
            MessageBox.Show(message, caption);
        }

        public bool ShowOpenFileDialog(FileDialogSettings settings)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = settings.InitialDirectory,
                Filter = settings.Filter
            };
            var dialogResult = ofd.ShowDialog();
            if (dialogResult == true)
            {
                settings.FileName = ofd.FileName;
                return true;
            }
            else
                return false;
        }

        public bool ShowSaveFileDialog(FileDialogSettings settings)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                InitialDirectory = settings.InitialDirectory,
                Filter = settings.Filter
            };
            var dialogResult = sfd.ShowDialog();
            if (dialogResult == true)
            {
                settings.FileName = sfd.FileName;
                return true;
            }
            else
                return false;
        }
    }
}

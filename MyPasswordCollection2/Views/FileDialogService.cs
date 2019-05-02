using Microsoft.Win32;
using MPC.ViewModels;
using System.IO;

namespace MPC.Views
{
    class FileDialogService : IDialogService
    {
        private readonly string Filter = "Password file(*.pw) | *.pw";

        private readonly string InitialDirectory = Directory.GetCurrentDirectory();

        public void ShowMessage(string message, string caption)
        {
            DialogWindow.Show(message, caption, DialogButtons.OK);
        }

        public bool ShowDialog(string message, string caption)
        {
            return DialogWindow.Show(message, caption, DialogButtons.YesNo);
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
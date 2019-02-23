using System;

namespace MPC.ViewModels
{
    public interface IDialogService
    {
        bool ShowOpenDialog(out string path);

        bool ShowSaveDialog(out string path);

        void ShowMessage(string message, string caption);

        bool ShowDialog(string message, string caption);
    }
}
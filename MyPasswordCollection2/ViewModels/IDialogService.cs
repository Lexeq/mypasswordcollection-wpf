using System;

namespace MPC.ViewModels
{
    public interface IDialogService
    {
        bool ShowOpenFileDialog(FileDialogSettings settings);

        bool ShowSaveFileDialog(FileDialogSettings settings);

        void ShowMessage(string message, string caption);

        bool ShowDialog(string message, string caption);
    }
}
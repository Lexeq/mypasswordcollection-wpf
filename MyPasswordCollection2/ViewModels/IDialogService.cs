using System;

namespace MPC.ViewModels
{
    public interface IDialogService
    {
        bool ShowOpenDialog(out string path);

        bool ShowSaveDialog(out string path);

        bool ShowMessageDialog(string message, string caption, DialogButtons buttons);
    }
}
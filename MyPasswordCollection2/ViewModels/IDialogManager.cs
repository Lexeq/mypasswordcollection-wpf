namespace MPC.ViewModels
{
    public interface IDialogManager
    {
        void ShowDialog(string key, object viewModel);
    }

    public static class DialogManagerExtensions
    {
        public static void Show<TViewModel>(this IDialogManager dialogManager, TViewModel viewModel)
        {
            var key = typeof(TViewModel).FullName;
            dialogManager.ShowDialog(key, viewModel);
        }
    }
}

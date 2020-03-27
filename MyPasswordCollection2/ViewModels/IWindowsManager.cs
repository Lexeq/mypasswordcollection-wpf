namespace MPC.ViewModels
{
    public interface IWindowsManager
    {
        void Show<TViewModel>(TViewModel vm);

        void ShowDialog<TViewModel>(TViewModel vm);
    }
}
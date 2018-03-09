using MPC.ViewModels;
using System.Windows;

namespace MPC
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DialogService dService = new DialogService();
            WindowsManager winManager = new WindowsManager();

            winManager.Register<InputWindowViewModel, InputWindow>();

            MainWindow mw = new MainWindow()
            {
                DataContext = new MainWindowViewModel(dService, winManager)
            };

            this.MainWindow = mw;
            mw.Show();
        }
    }
}

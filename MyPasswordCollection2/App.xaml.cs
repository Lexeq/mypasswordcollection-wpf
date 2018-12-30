using MPC.Model.Repository;
using MPC.ViewModels;
using MPC.Views;
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
            FileRepositoryManager fileManager = new FileRepositoryManager();

            winManager.Register<InputWindowViewModel, InputWindow>();
            winManager.Register<AboutViewModel, AboutWindow>();

            MainWindow mw = new MainWindow()
            {
                DataContext = new MainWindowViewModel(dService, winManager, fileManager)
            };

            this.MainWindow = mw;
            mw.Show();
        }
    }
}

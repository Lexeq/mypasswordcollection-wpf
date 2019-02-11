using MPC.Model.Repository;
using MPC.ViewModels;
using MPC.Views;
using System;
using System.IO;
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
            Current.Dispatcher.UnhandledException += LogException;
            DialogService dService = new DialogService();
            WindowsManager winManager = new WindowsManager();
            FileRepositoryManager fileManager = new FileRepositoryManager();

            winManager.Register<InputWindowViewModel, InputWindow>();
            winManager.Register<AboutViewModel, AboutWindow>();
            winManager.Register<ExceptionViewModel, ExceptionWindow>();

            MainWindow mw = new MainWindow()
            {
                DataContext = new MainWindowViewModel(dService, winManager, fileManager)
            };

            this.MainWindow = mw;
            mw.Show();
        }

        private void LogException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            File.AppendAllText(path, DateTime.Now.ToString() + Environment.NewLine + e.Exception.ToString());
        }
    }
}

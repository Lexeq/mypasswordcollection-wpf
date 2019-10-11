using MPC.Model.Repository;
using MPC.ViewModels;
using MPC.Views;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Linq;

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
            FileDialogService dService = new FileDialogService();
            WindowsManager winManager = new WindowsManager();
            FileRepositoryManager fileManager = new FileRepositoryManager();

            winManager.Register<InputWindowViewModel, InputWindow>();
            winManager.Register<AboutViewModel, AboutWindow>();
            winManager.Register<ExceptionViewModel, ExceptionWindow>();
            winManager.Register<PasswordGenerationViewModel, PasswordGenerationWindows>();

            var mainVM = new MainWindowViewModel(dService, winManager, fileManager);
            MainWindow mw = new MainWindow()
            {
                DataContext = mainVM
            };

            this.MainWindow = mw;
            mw.Show();

            var defPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "passwords.pw");
            if (File.Exists(defPath))
            {
                var met = mainVM.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).First(mi => mi.Name == "OpenRepository" && mi.GetParameters().Length == 1);
                met.Invoke(mainVM, new[] { defPath });
            }
        }

        private void LogException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            File.AppendAllText(path, DateTime.Now.ToString() + Environment.NewLine + e.Exception.ToString());
        }
    }
}

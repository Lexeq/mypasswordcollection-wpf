using MPC.Model.Repository;
using MPC.ViewModels;
using MPC.Views;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Linq;
using System.Globalization;

namespace MPC
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            InitializeResourceDictionary();

            Current.Dispatcher.UnhandledException += LogException;
            WindowsManager winManager = new WindowsManager();
            FileRepositoryManager fileManager = new FileRepositoryManager();
            IStringsSource ss = new TextService(Resources);

            winManager.Register<InputWindowViewModel, InputWindow>();
            winManager.Register<AboutViewModel, AboutWindow>();
            winManager.Register<ExceptionViewModel, ExceptionWindow>();
            winManager.Register<PasswordGenerationViewModel, PasswordGenerationWindows>();

            var mainVM = new MainWindowViewModel(winManager, winManager, fileManager, ss);
            MainWindow mw = new MainWindow()
            {
                DataContext = mainVM
            };

            this.MainWindow = mw;
            mw.Show();

            var defPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "passwords.pw");
            if (File.Exists(defPath))
            {
                 mainVM.OpenPathCommand.Execute(defPath);
            }
        }

        private void InitializeResourceDictionary()
        {
            ResourceDictionary localizatedStrings = new ResourceDictionary();
            switch (CultureInfo.CurrentCulture.Name)
            {
                case "ru-RU":
                    localizatedStrings.Source = new Uri("..\\Resources\\lang.ru-ru.xaml", UriKind.Relative);
                    break;
            }
            Resources.MergedDictionaries.Add(localizatedStrings);
        }

        private void LogException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
            File.AppendAllText(path, DateTime.Now.ToString() + Environment.NewLine + e.Exception.ToString());
        }
    }
}

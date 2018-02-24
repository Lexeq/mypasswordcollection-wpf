using MPC.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            base.OnStartup(e);
            var dialogManager = new DialogManager();
            dialogManager.Register<InputWindowViewModel, InputWindow>();

            var mainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(new MessageService(), new IOService(), new DialogManager()),
            };
            mainWindow.Show();
        }
    }
}

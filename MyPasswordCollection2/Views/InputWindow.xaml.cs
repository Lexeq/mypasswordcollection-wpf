using System.Windows;
using System.Windows.Input;

namespace MPC
{
    /// <summary>
    /// Логика взаимодействия для InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(passBox.Password) && (confBox.Visibility != Visibility.Visible || passBox.Password == confBox.Password))
            {
                if (DataContext != null)
                {
                    ICommand cmd = DataContext.GetType()
                        .GetProperty("OkCommand")
                        .GetValue(DataContext)
                        as ICommand;

                    if (cmd != null && cmd.CanExecute(null))
                    {
                        cmd.Execute(passBox.Password);
                    }
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Passwords are not same");
            }
        }
    }
}

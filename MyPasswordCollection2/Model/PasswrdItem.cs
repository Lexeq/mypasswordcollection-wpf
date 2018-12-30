using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MPC.Model
{
    public class PasswordItem : INotifyPropertyChanged
    {
        private string site;
        private string login;
        private string password;

        public virtual string Site
        {
            get => site; set
            {
                site = value;
          //      OnPropertyChanged();
            }
        }

        public virtual string Login
        {
            get => login;
            set
            {
                login = value;
              //  OnPropertyChanged();
            }
        }

        public virtual string Password
        {
            get => password; set
            {
                password = value;
               // OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName]string propName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public override string ToString()
        {
            return Site;
        }
    }
}

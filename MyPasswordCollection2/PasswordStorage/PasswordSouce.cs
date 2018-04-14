using System;
using System.IO;
using System.Collections.ObjectModel;

namespace PasswordStorage
{
    sealed class PasswordSource
    {
        private string password;

        private PasswordsCrypter crypter;

        public bool IsSyncWithFile { get; private set; }

        public ObservableCollection<PasswordItem> Passwords { get; private set; }

        public string FilePath { get; private set; }

        public PasswordSource(string path, string password)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException(nameof(path));

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException(nameof(password));

            this.password = password;
            crypter = new PasswordsCrypter();
            FilePath = path;

            if (File.Exists(path))
            {
                Passwords = new ObservableCollection<PasswordItem>(crypter.Decrypt(File.ReadAllBytes(FilePath), this.password));
            }
            else
            {
                Passwords = new ObservableCollection<PasswordItem>();
                SaveToFile();
            }

            Passwords.CollectionChanged += Passwords_CollectionChanged;
        }

        private void Passwords_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                SaveToFile();
                IsSyncWithFile = true;
            }
            catch
            {
                IsSyncWithFile = false;
            }
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            if (oldPassword != password)
                throw new ArgumentException("Wrong password");

            password = newPassword;
            try
            {
                SaveToFile();
            }
            catch
            {
                password = oldPassword;
                throw;
            }
        }

        public void SaveToFile()
        {
            File.WriteAllBytes(FilePath, crypter.Encrypt(Passwords, password));
        }
    }
}

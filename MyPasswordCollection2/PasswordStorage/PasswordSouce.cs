using System;
using System.IO;
using System.Collections.ObjectModel;

namespace PasswordStorage
{
    sealed class PasswordSource
    {
        public bool IsSyncWithFile { get; private set; }

        public ObservableCollection<PasswordItem> Passwords { get; private set; }

        private string password;

        public string FilePath { get; private set; }

        public PasswordsCrypter crypter;

        public PasswordSource(string path, string password)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("path");

            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password");

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

            Passwords.CollectionChanged += (o, e) => SaveToFile();
        }

        public void SaveToFile()
        {
            try
            {
                File.WriteAllBytes(FilePath, crypter.Encrypt(Passwords, password));
                IsSyncWithFile = true;
            }
            catch
            {
                IsSyncWithFile = false;
            }
        }
    }
}

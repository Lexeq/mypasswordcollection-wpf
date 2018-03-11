using System;
using System.IO;
using System.Collections.ObjectModel;

namespace PasswordStorage
{
    sealed class PasswordSource
    {
        public bool SyncWithFile { get; private set; }

        public ObservableCollection<PasswordItem> Collection { get; private set; }

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
                Collection = new ObservableCollection<PasswordItem>(crypter.Decrypt(File.ReadAllBytes(FilePath), this.password));
            }
            else
            {
                Collection = new ObservableCollection<PasswordItem>();
                SaveToFile();
            }

            Collection.CollectionChanged += (o, e) => SaveToFile();
        }

        public void SaveToFile()
        {
            try
            {
                File.WriteAllBytes(FilePath, crypter.Encrypt(Collection, password));
                SyncWithFile = true;
            }
            catch
            {
                SyncWithFile = false;
            }
        }
    }

}

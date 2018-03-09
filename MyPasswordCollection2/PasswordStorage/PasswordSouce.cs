﻿using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;

namespace PasswordStorage
{
    class PasswordSource
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

            try
            {
                Collection = new ObservableCollection<PasswordItem>(crypter.Decrypt(File.ReadAllBytes(FilePath), this.password));
            }
            catch (Exception ex)
            {
                throw new FileLoadException("Can't load file", ex);
            }
            Collection.CollectionChanged += Collection_CollectionChanged;
        }

        private void Collection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            WriteToFile();
        }

        public void WriteToFile()
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

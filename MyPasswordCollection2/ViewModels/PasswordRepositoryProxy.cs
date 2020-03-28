using MPC.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MPC.ViewModels
{
    internal class PasswordRepositoryProxy : IPasswordRepository, IList, INotifyCollectionChanged
    {
        public IPasswordRepository Original { get; }

        public PasswordRepositoryProxy(IPasswordRepository original)
        {
            this.Original = original ?? throw new ArgumentNullException(nameof(original));
        }

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var handler = CollectionChanged;
            handler?.Invoke(this, e);
        }

        #endregion

        //Implementation IPasswordRepository with INotifyCollectionChanged
        #region IPasswordRepository

        public PasswordItem this[int index] => Original[index];

        public void ChangePassword(string oldPassword, string newPassword) => Original.ChangePassword(oldPassword, newPassword);

        public int Count => Original.Count;

        public void Clear()
        {
            Original.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Remove(PasswordItem item)
        {
            if (Original.Remove(item))
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            }
            return false;
        }

        public void Save(PasswordItem item)
        {
            bool isNewItem = !Original.Contains(item);

            Original.Save(item);
            var args = isNewItem ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item) : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, item);

            OnCollectionChanged(args);
        }

        public IEnumerator<PasswordItem> GetEnumerator() => Original.GetEnumerator();

        public void Dispose() => Original.Dispose();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Original).GetEnumerator();

        #endregion

        //IList is necessary for using PasswordRepositoryProxy with ListCollectionView
        #region IList
        object IList.this[int index] { get => this[index]; set => throw new NotSupportedException("Use IPasswordRepository.Save instead."); }

        void IList.Remove(object value) => Remove((PasswordItem)value);

        void IList.RemoveAt(int index) => Remove(this[index]);

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        int IList.IndexOf(object value)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i] == value)
                {
                    return i;
                }
            }
            return -1;
        }

        bool IList.Contains(object value) => value is PasswordItem && (this as IList).IndexOf(value) >= 0;

        int IList.Add(object value)
        {
            Save((PasswordItem)value);
            return Count - 1;
        }

        void IList.Clear()
        {
            Clear();
        }

        bool IList.IsReadOnly => true;

        bool IList.IsFixedSize => false;

        object ICollection.SyncRoot => throw new NotSupportedException();

        bool ICollection.IsSynchronized => throw new NotSupportedException();

        void ICollection.CopyTo(Array array, int index) => this.ToArray().CopyTo(array, index);
        #endregion
    }
}
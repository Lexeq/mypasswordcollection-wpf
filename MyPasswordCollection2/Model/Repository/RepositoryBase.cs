using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MPC.Model.Repository
{
    internal class RepositoryBase : IPasswordRepository
    {
        private static ICrypter CreateCrypter(string password)
        {
            return new AesCrypter(password);
        }

        private readonly List<PasswordItem> items;
        private readonly Stream stream;
        private readonly long origin;

        private bool disposed;
        private PasswordItemsStorage storage;

        internal RepositoryBase(Stream stream, string password)
        {
            this.stream = stream;
            origin = stream.Position;
            var crypter = CreateCrypter(password);

            if (stream.Length == stream.Position)
            {
                InitializeNewRepository(crypter);
            }
            else
            {
                try
                {
                    CheckCrypter(crypter);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }
            this.storage = new PasswordItemsStorage(stream, new CryptoConverter(crypter));
            items = storage.ReadAll().ToList();
        }

        private void InitializeNewRepository(ICrypter crypter)
        {
            stream.Position = origin;
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode, true))
            {
                var cryptedBlock = crypter.Encrypt(Array.Empty<byte>());
                writer.Write(cryptedBlock.Length);
                writer.Write(cryptedBlock);
            }
        }

        private bool CheckCrypter(ICrypter crypter)
        {
            stream.Position = origin;
            using (BinaryReader reader = new BinaryReader(stream, Encoding.Unicode, true))
            {
                var testBlockSize = reader.ReadInt32();
                var testBlock = reader.ReadBytes(testBlockSize);
                return crypter.Decrypt(testBlock).Length == 0;
            }
        }

        #region IPasswordRepository
        public PasswordItem this[int index] => items[index];

        public int Count => items.Count;

        public void Clear()
        {
            items.Clear();
            storage.WriteAll(items);
        }

        public bool Remove(PasswordItem item)
        {
            var index = items.IndexOf(item);
            if (index >= 0)
            {
                items.Remove(item);
                storage.WriteAll(items);
                return true;
            }
            return false;
        }

        public void Save(PasswordItem item)
        {
            int index = items.IndexOf(item);
            if (index < 0)
            {
                items.Add(item);
                storage.Append(item);
            }
            else
            {
                storage.WriteAll(items);
            }
        }

        public bool ChangePassword(string oldPassword, string newPassword)
        {
            var oldCrypter = CreateCrypter(oldPassword);

            try
            {
                if (CheckCrypter(oldCrypter))
                {
                    var crypter = new AesCrypter(newPassword);
                    InitializeNewRepository(crypter);
                    storage = new PasswordItemsStorage(stream, new CryptoConverter(new AesCrypter(newPassword)));
                    storage.WriteAll(items);
                    return true;
                }
            }
            catch (CryptographicException)
            {
                return false;
            }

            return false;
        }

        public IEnumerator<PasswordItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    stream?.Dispose();
            }
            disposed = true;
        }
        #endregion
    }
}

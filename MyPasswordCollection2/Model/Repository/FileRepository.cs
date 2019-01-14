﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MPC.Model.Repository
{
    sealed class FileRepository : IPasswordRepository
    {
        private const int Header = 547493281;

        private long dataBlockOffset;

        private Stream stream;

        private ICrypter crypter;

        private List<PasswordItem> items;

        public FileRepository(Stream stream, string password)
        {
            crypter = new AesCrypter(password);
            this.stream = stream;
            stream.Position = 0;
            if (stream.Length != 0)
            {
                using (BinaryReader reader = new BinaryReader(stream, Encoding.Unicode, true))
                {
                    //check header
                    if (BitConverter.ToInt32(reader.ReadBytes(4), 0) != Header)
                        throw new ArgumentException("Invalid file");

                    //check password
                    var testBlockSize = reader.ReadInt32();
                    byte[] check;
                    try
                    {
                        check = crypter.Decrypt(reader.ReadBytes(testBlockSize));
                        if (BitConverter.ToInt32(check, 0) != Header)
                            throw new RepositoryException("Incorrect check value");
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException("Invalid password", ex);
                    }
                }

                dataBlockOffset = stream.Position;
                items = ReadFile();
            }
            else
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode, true))
                {
                    writer.Write(Header);
                    var check = crypter.Encrypt(BitConverter.GetBytes(Header));
                    writer.Write(check.Length);
                    writer.Write(check);
                }
                dataBlockOffset = stream.Position;
                items = new List<PasswordItem>();
            }
        }

        #region IPasswordRepository

        public PasswordItem this[int index] => items[index];

        public int Count => items.Count;

        public void Clear()
        {
            items.Clear();
            WriteFile();
        }

        public bool Remove(PasswordItem item)
        {
            var index = items.IndexOf(item);
            if (index >= 0)
            {
                items.Remove(item);
                WriteFile();
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
            }
            WriteFile();
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
            stream?.Dispose();
        }

        #endregion

        private List<PasswordItem> ReadFile()
        {
            var items = new List<PasswordItem>();

            using (BinaryReader reader = new BinaryReader(stream, Encoding.Unicode, true))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var length = reader.ReadInt32();
                    var itemDataEnc = reader.ReadBytes(length);
                    var itemDataDec = crypter.Decrypt(itemDataEnc);

                    items.Add(PasswordItemFromBytes(itemDataDec));
                }
            }

            return items;
        }

        private void WriteFile()
        {
            try
            {
                stream.Position = dataBlockOffset;

                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode, true))
                {
                    foreach (var item in items)
                    {
                        var itemBytes = crypter.Encrypt(PasswordItemToBytes(item));
                        writer.Write(BitConverter.GetBytes(itemBytes.Length));
                        writer.Write(itemBytes);
                    }
                }
                stream.SetLength(stream.Position);
                stream.Flush();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An exception has occurred while saving data.", ex);
            }
        }

        private byte[] PasswordItemToBytes(PasswordItem item)
        {
            if (item == null)
                throw new NullReferenceException("PasswordItem is null");
            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms, Encoding.Unicode))
            {
                writer.Write(item.Site);
                writer.Write(item.Login);
                writer.Write(item.Password);
                return ms.ToArray();
            }
        }

        private PasswordItem PasswordItemFromBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            PasswordItem item = new PasswordItem();

            using (MemoryStream ms = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(ms, Encoding.Unicode))
            {
                item.Site = reader.ReadString();
                item.Login = reader.ReadString();
                item.Password = reader.ReadString();
            }
            return item;
        }
    }
}

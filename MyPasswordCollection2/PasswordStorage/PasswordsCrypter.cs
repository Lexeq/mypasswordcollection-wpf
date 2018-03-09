using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordStorage
{
    public class PasswordsCrypter : IPasswordsCrypter
    {
        private readonly Random random = new Random();

        private const int HEADER = 537413289;

        public byte[] Encrypt(IEnumerable<PasswordItem> items, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("String is null or empty", nameof(password));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            byte[] iv = null;
            byte[] key = null;
            byte[] salt = GetRandomSalt();

            GetKeyAndIVFromPassword(password, salt, ref key, ref iv);

            List<byte> data = new List<byte>();
            foreach (var item in items)
            {
                if (item == null)
                    throw new NullReferenceException("items have a null element");

                data.AddRange(PasswordItemToBytes(item));
            }
            var encbytes = EncryptBytes(data.ToArray(), key, iv);

            List<byte> resBytes = new List<byte>();

            resBytes.AddRange(BitConverter.GetBytes(HEADER));
            resBytes.AddRange(salt);
            resBytes.AddRange(encbytes);

            return resBytes.ToArray();
        }

        public PasswordItem[] Decrypt(byte[] bytes, string password)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("String is null or empty", nameof(password));

            if (bytes.Length < 20 || BitConverter.ToInt32(TakeRange(bytes, 0, 4), 0) != HEADER)
                throw new InvalidDataException("Invalid file.");

            List<PasswordItem> list = new List<PasswordItem>();

            var salt = TakeRange(bytes, 4, 8);

            byte[] key = null;
            byte[] iv = null;

            GetKeyAndIVFromPassword(password, salt, ref key, ref iv);

            var dbytes = DecryptBytes(TakeRange(bytes, 12, bytes.Length - 12), key, iv);

            for (int i = 0; i < dbytes.Length;)
            {
                try
                {
                    int siteLen = BitConverter.ToInt32(TakeRange(dbytes, i, 4), 0);
                    int logLen = BitConverter.ToInt32(TakeRange(dbytes, i += 4, 4), 0);
                    int pasLen = BitConverter.ToInt32(TakeRange(dbytes, i += 4, 4), 0);

                    string site = Encoding.Unicode.GetString(TakeRange(dbytes, i += 4, siteLen));
                    string login = Encoding.Unicode.GetString(TakeRange(dbytes, i += siteLen, logLen));
                    string pas = Encoding.Unicode.GetString(TakeRange(dbytes, i += logLen, pasLen));

                    list.Add(new PasswordItem(site, login, pas));

                    i += pasLen;
                }
                catch (ArgumentException e)
                {
                    throw new InvalidDataException("File corrupted", e);
                }
            }

            return list.ToArray();
        }

        private byte[] PasswordItemToBytes(PasswordItem item)
        {
            var sbts = Encoding.Unicode.GetBytes(item.Site);
            var lbts = Encoding.Unicode.GetBytes(item.Login);
            var pbts = Encoding.Unicode.GetBytes(item.Password);

            List<byte> bts = new List<byte>(12 + sbts.Length + lbts.Length + pbts.Length);

            bts.AddRange(BitConverter.GetBytes(sbts.Length));
            bts.AddRange(BitConverter.GetBytes(lbts.Length));
            bts.AddRange(BitConverter.GetBytes(pbts.Length));
            bts.AddRange(sbts);
            bts.AddRange(lbts);
            bts.AddRange(pbts);

            return bts.ToArray();
        }

        private byte[] TakeRange(byte[] source, int start, int length)
        {
            if (start + length > source.Length)
                throw new ArgumentException("Length is greater than the number of elements from start to the end of source.");

            byte[] res = new byte[length];
            Array.Copy(source, start, res, 0, res.Length);
            return res;
        }

        private byte[] EncryptBytes(byte[] data, byte[] key, byte[] IV)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (IV == null)
                throw new ArgumentNullException(nameof(IV));

            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            return encrypted;
        }

        private byte[] DecryptBytes(byte[] data, byte[] key, byte[] IV)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (IV == null)
                throw new ArgumentNullException(nameof(IV));

            byte[] decr;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var msDecrypt = new MemoryStream(data))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            csDecrypt.CopyTo(ms);
                            decr = ms.ToArray();
                        }
                    }
                }
            }

            return decr;
        }

        private void GetKeyAndIVFromPassword(string password, byte[] salt, ref byte[] key, ref byte[] iv)
        {
            Rfc2898DeriveBytes rfc2898DeriveBytes =
                new Rfc2898DeriveBytes(password, salt);
            key = rfc2898DeriveBytes.GetBytes(32);
            iv = rfc2898DeriveBytes.GetBytes(16);
        }

        private byte[] GetRandomSalt()
        {
            var salt = new byte[8];
            random.NextBytes(salt);
            return salt;
        }
    }
}

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace MPC.Model.Repository
{
    class AesCrypter : ICrypter
    {
        private const int SaltLength = 8;

        private static readonly Random random = new Random();

        private string password;

        public AesCrypter(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (password.Length == 0)
                throw new ArgumentException(nameof(password));

            this.password = password;
        }

        public byte[] Decrypt(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var salt = new byte[SaltLength];
            Array.Copy(data, salt, SaltLength);

            GetKeyAndIVFromPassword(salt, out byte[] key, out byte[] iv);

            var encrypted = new byte[data.Length - salt.Length];

            Array.Copy(data, salt.Length, encrypted, 0, encrypted.Length);
            var decrypted = DecryptBytes(encrypted, key, iv);

            return decrypted;
        }

        public byte[] Encrypt(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var salt = GetRandomSalt();
            GetKeyAndIVFromPassword(salt, out byte[] key, out byte[] iv);
            var encrypted = EncryptBytes(data, key, iv);

            byte[] res = new byte[salt.Length + encrypted.Length];
            salt.CopyTo(res, 0);
            encrypted.CopyTo(res, salt.Length);

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

        private void GetKeyAndIVFromPassword(byte[] salt, out byte[] key, out byte[] iv)
        {
            Rfc2898DeriveBytes rfc2898DeriveBytes =
                new Rfc2898DeriveBytes(password, salt);
            key = rfc2898DeriveBytes.GetBytes(32);
            iv = rfc2898DeriveBytes.GetBytes(16);
        }

        private byte[] GetRandomSalt()
        {
            var salt = new byte[SaltLength];
            random.NextBytes(salt);
            return salt;
        }
    }
}

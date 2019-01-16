using System;
using System.IO;
using System.Security.Cryptography;

namespace MPC.Model.Repository
{
    internal sealed class AesCrypter : ICrypter
    {
        private readonly byte[] salt = new byte[16]
        { 32, 8, 22, 58, 67, 150, 213, 230, 200, 56, 115, 136, 142, 18, 217, 63 };

        private byte[] key;
        private byte[] iv;
        
        public AesCrypter(string password, byte[] salt = null)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (password.Length == 0)
                throw new ArgumentException("Password can't be empty.", nameof(password));
            if (salt != null)
                this.salt = salt;

            GetKeyAndIVFromPassword(password, this.salt);
        }

        public byte[] Encrypt(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor();

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                    }
                    return ms.ToArray();
                }
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor();

                using (var dataStream = new MemoryStream(data))
                {
                    using (var cs = new CryptoStream(dataStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream resultStream = new MemoryStream())
                        {
                            cs.CopyTo(resultStream);
                            return resultStream.ToArray();
                        }
                    }
                }
            }
        }

        private void GetKeyAndIVFromPassword(string password, byte[] salt)
        {
            Rfc2898DeriveBytes rfc2898DeriveBytes =
                new Rfc2898DeriveBytes(password, salt);
            key = rfc2898DeriveBytes.GetBytes(32);
            iv = rfc2898DeriveBytes.GetBytes(16);
        }
    }
}

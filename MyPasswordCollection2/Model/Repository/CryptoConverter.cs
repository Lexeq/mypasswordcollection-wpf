using System;
using System.IO;
using System.Text;

namespace MPC.Model.Repository
{
    internal sealed class CryptoConverter : IPasswordConverter
    {
        private readonly ICrypter crypter;

        public CryptoConverter(ICrypter crypter)
        {
            this.crypter = crypter ?? throw new ArgumentNullException(nameof(crypter));
        }

        public byte[] ToBytes(PasswordItem item)
        {
            if (item == null)
                throw new ArgumentNullException("PasswordItem is null");

            using (MemoryStream ms = new MemoryStream())
            {
                WriteCryptedString(ms, item.Site);
                WriteCryptedString(ms, item.Login);
                WriteCryptedString(ms, item.Password);
                return ms.ToArray();
            }
        }

        public PasswordItem FromBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            PasswordItem item = new PasswordItem();

            using (MemoryStream ms = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(ms, Encoding.Unicode))
            {
                var siteLength = reader.ReadInt32();
                item.Site = DecryptString(reader.ReadBytes(siteLength));

                var loginLength = reader.ReadInt32();
                item.Login = DecryptString(reader.ReadBytes(loginLength));

                var passwordLength = reader.ReadInt32();
                item.Password = DecryptString(reader.ReadBytes(passwordLength));

                if (ms.Position != bytes.Length)
                    throw new ArgumentException("Invalid data. Array is to big.");
            }

            return item;
        }

        private void WriteCryptedString(Stream stream, string str)
        {
            var bytes = crypter.Encrypt(Encoding.Unicode.GetBytes(str));
            stream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            stream.Write(bytes,0,bytes.Length);
        }

        private string DecryptString(byte[] data)
        {
            return Encoding.Unicode.GetString(crypter.Decrypt(data));
        }
    }
}

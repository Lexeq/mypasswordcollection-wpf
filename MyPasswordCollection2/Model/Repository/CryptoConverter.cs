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
            using (BinaryWriter writer = new BinaryWriter(ms, Encoding.Unicode))
            {
                var siteBytes = crypter.Encrypt(Encoding.Unicode.GetBytes(item.Site));
                var loginBytes = crypter.Encrypt(Encoding.Unicode.GetBytes(item.Login));
                var passwordBytes = crypter.Encrypt(Encoding.Unicode.GetBytes(item.Password));

                writer.Write(siteBytes.Length);
                writer.Write(siteBytes);

                writer.Write(loginBytes.Length);
                writer.Write(loginBytes);

                writer.Write(passwordBytes.Length);
                writer.Write(passwordBytes);

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
                item.Site = Encoding.Unicode.GetString(crypter.Decrypt(reader.ReadBytes(siteLength)));

                var loginLength = reader.ReadInt32();
                item.Login = Encoding.Unicode.GetString(crypter.Decrypt(reader.ReadBytes(loginLength)));

                var passwordLength = reader.ReadInt32();
                item.Password = Encoding.Unicode.GetString(crypter.Decrypt(reader.ReadBytes(passwordLength)));

                if (ms.Position != bytes.Length)
                    throw new ArgumentException("Invalid data. Array is to big.");
            }

            return item;
        }
    }
}

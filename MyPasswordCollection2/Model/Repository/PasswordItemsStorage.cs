using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MPC.Model.Repository
{
    internal sealed class PasswordItemsStorage : IDisposable
    {
        private readonly long origin;

        private readonly Stream stream;

        private readonly IPasswordConverter converter;

        private bool disposed = false;

        public PasswordItemsStorage(Stream stream, IPasswordConverter converter)
        {
            this.stream = stream;
            this.converter = converter;
            origin = stream.Position;
        }

        public PasswordItem[] ReadAll()
        {
            stream.Position = origin;

            List<PasswordItem> items = new List<PasswordItem>();

            using (BinaryReader reader = new BinaryReader(stream, Encoding.Unicode, true))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var length = reader.ReadInt32();
                    var itemBytes = reader.ReadBytes(length);

                    items.Add(converter.FromBytes(itemBytes));
                }
            }

            return items.ToArray();
        }

        public void Append(PasswordItem item)
        {
            stream.Position = stream.Length;
            Write(item);
            stream.Flush();
        }

        public void WriteAll(IEnumerable<PasswordItem> items)
        {
            stream.Position = origin;

            foreach (var item in items)
            {
                Write(item);
            }
            stream.SetLength(stream.Position);
            stream.Flush();
        }

        private void Write(PasswordItem item)
        {
            var bytes = converter.ToBytes(item);
            stream.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            stream.Write(bytes, 0, bytes.Length);
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
    }
}

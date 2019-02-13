using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MPC.Model.Repository
{
    internal sealed class FileRepository : StreamRepository
    {
        private const int Header = 547493281;

        #region Static

        public static FileRepository Create(string path, string password)
        {
            var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using (BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode, true))
            {
                writer.Write(Header);
            }
            return new FileRepository(path, stream, password);
        }

        public static FileRepository Open(string path, string password)
        {
            var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            if (!CheckFileHeader(stream))
            {
                stream.Dispose();
                throw new RepositoryException("Invalid file.");
            }
            return new FileRepository(path, stream, password);
        }

        private static bool CheckFileHeader(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream, Encoding.Unicode, true))
            {
                return stream.Length > 4 && reader.ReadInt32() == Header;
            }
        }

        #endregion

        public string Path { get; }

        private FileRepository(string path, Stream stream, string password) :
            base(stream, password)
        {
            Path = path;
        }
    }
}

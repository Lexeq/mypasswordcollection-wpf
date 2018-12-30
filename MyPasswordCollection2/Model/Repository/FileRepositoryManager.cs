using System.IO;

namespace MPC.Model.Repository
{
    class FileRepositoryManager : IRepositoryManager
    {
        public IPasswordRepository GetRepository(string path, string password)
        {
            FileStream stream;

            if (File.Exists(path))
            {
                stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            else
            {
                stream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
            }
            return new FileRepository(stream, password);
        }
    }
}

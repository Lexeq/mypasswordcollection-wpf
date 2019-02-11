using System;
using System.IO;
using System.Security.Cryptography;
namespace MPC.Model.Repository
{
    class FileRepositoryManager : IRepositoryManager
    {
        public IPasswordRepository Create(string path, string password)
        {
            try
            {
                return FileRepository.Create(path, password);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't create repository.", ex);
            }
        }

        public IPasswordRepository Get(string path, string password)
        {
            try
            {
                return FileRepository.Open(path, password);
            }
            catch (PasswordException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Can't open repository.", ex);
            }
        }

        public void DeleteRepository(IPasswordRepository repository)
        {
            if (!(repository is FileRepository fileRepo))
                throw new ArgumentException($"{nameof(FileRepositoryManager)} can delete only {nameof(FileRepository)}");
            string path = fileRepo.Path;
            fileRepo.Dispose();
            File.Delete(path);
        }
    }
}
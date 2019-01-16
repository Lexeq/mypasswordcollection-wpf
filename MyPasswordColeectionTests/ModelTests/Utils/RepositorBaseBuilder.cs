using MPC.Model;
using MPC.Model.Repository;
using System.IO;

namespace MyPasswordColeectionTests.ModelTests.Utils
{
    class RepositorBaseBuilder
    {
        private string password;
        private int itemsCount;

        public MemoryStream Stream { get; private set; }

        public RepositorBaseBuilder()
        {
            password = "test";
            itemsCount = 0;
            Stream = new MemoryStream();
        }

        public RepositorBaseBuilder WithPassword(string password)
        {
            this.password = password;
            return this;
        }

        public RepositorBaseBuilder WithItemsCount(int count)
        {
            itemsCount = count;
            return this;
        }

        public RepositorBaseBuilder FromStream(MemoryStream stream)
        {
            Stream = stream;
            return this;
        }

        public RepositoryBase Build()
        {
            var repo = new RepositoryBase(Stream, password);
            FillRepository(repo);
            return repo;
        }

        private void FillRepository(RepositoryBase repo)
        {
            for (int i = 1; i <= itemsCount; i++)
            {
                repo.Save(new PasswordItem { Site = $"Site_{i}", Login = $"Login_{i}", Password = $"Password_{i}" });
            }
        }
    }
}

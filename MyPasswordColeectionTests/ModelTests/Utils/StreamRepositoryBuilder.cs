using MPC.Model;
using MPC.Model.Repository;
using System.IO;

namespace MyPasswordColeectionTests.ModelTests.Utils
{
    class StreamRepositoryBuilder
    {
        private string password;
        private int itemsCount;

        public MemoryStream Stream { get; private set; }

        public StreamRepositoryBuilder()
        {
            password = "test";
            itemsCount = 0;
            Stream = new MemoryStream();
        }

        public StreamRepositoryBuilder WithPassword(string password)
        {
            this.password = password;
            return this;
        }

        public StreamRepositoryBuilder WithItemsCount(int count)
        {
            itemsCount = count;
            return this;
        }

        public StreamRepositoryBuilder FromStream(MemoryStream stream)
        {
            Stream = stream;
            return this;
        }

        public StreamRepository Build()
        {
            var repo = new StreamRepository(Stream, password);
            FillRepository(repo);
            return repo;
        }

        private void FillRepository(StreamRepository repo)
        {
            for (int i = 1; i <= itemsCount; i++)
            {
                repo.Save(new PasswordItem { Site = $"Site_{i}", Login = $"Login_{i}", Password = $"Password_{i}" });
            }
        }
    }
}

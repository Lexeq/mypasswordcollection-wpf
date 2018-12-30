using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MPC.Model.Repository;
using System.IO;
using MPC.Model;

namespace MyPasswordColeectionTests.ModelTests
{
    [TestFixture]
    class RepositoryTests
    {
        const string password = "testpassword11$$\"";
        [Test]
        public void NewRepository()
        {
            var ms = new MemoryStream();

            Assert.DoesNotThrow(() => {
                FileRepository repo = new FileRepository(ms, "password");
            });
        }

        [Test]
        public void OpenRepository()
        {
            var ms = new MemoryStream();
            FileRepository newRepo = new FileRepository(ms, password);
            newRepo.Save(new PasswordItem() { Login = "L1", Password = "P1", Site = "S1" });
            newRepo.Save(new PasswordItem() { Login = "L2", Password = "P2", Site = "S2" });
            newRepo.Save(new PasswordItem() { Login = "L3", Password = "P3", Site = "S3" });

            var openedRepo = new FileRepository(ms, password);
            Assert.AreEqual(3, openedRepo.Count);
        }

        [Test]
        public void AddNew()
        {
            var ms = new MemoryStream();

            FileRepository repo = new FileRepository(ms, "Keka");

            repo.Save(new PasswordItem() { Login = "L1", Password = "P1", Site = "S1" });
            Assert.AreEqual(1, repo.Count);
        }

        [Test]
        public void UpdateItem()
        {
            var ms = new MemoryStream();
            FileRepository newRepo = new FileRepository(ms, password);
            newRepo.Save(new PasswordItem() { Login = "L1", Password = "P1", Site = "S1" });
            newRepo.Save(new PasswordItem() { Login = "L2", Password = "P2", Site = "S2" });
            newRepo.Save(new PasswordItem() { Login = "L3", Password = "P3", Site = "S3" });

            var openedRepo = new FileRepository(ms, password);
            var item = openedRepo[1];
            item.Login = "XXX";
            openedRepo.Save(item);

            var tt = ms.ToArray();
            ms.Dispose();

            var brandNewRepo = new FileRepository(new MemoryStream(tt), password);
            Assert.AreEqual("XXX", brandNewRepo[1].Login);
        }
    }
}

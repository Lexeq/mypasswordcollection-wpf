using NUnit.Framework;
using System.Linq;
using System.IO;
using MPC.Model;
using MyPasswordColeectionTests.ModelTests.Utils;

namespace MyPasswordColeectionTests.ModelTests
{

    [TestFixture]
    class RepositoryTests
    {
        RepositorBaseBuilder builder;

        [SetUp]
        public void Init()
        {
            builder = new RepositorBaseBuilder();
        }

        [Test]
        public void CreateNewRepositoryTest()
        {
            var repo = builder.Build();

            Assert.AreEqual(0, repo.Count);
        }

        [Test]
        public void Save_NewItem_ItemAdded()
        {
            var repo = builder.Build();

            var item = new PasswordItem() { Login = "L1", Password = "P1", Site = "S1" };
            repo.Save(item);

            Assert.AreEqual(1, repo.Count);
            Assert.True(new PasswordItemEqualityComparer().Equals(item, repo.First()));
        }

        [Test]
        public void Save_ExistingItem_ItemUpdated()
        {
            var repo = builder.WithItemsCount(3).Build();
            var item = repo[1];

            item.Login = "XXX";
            repo.Save(item);

            Assert.AreEqual("XXX", repo[1].Login);
            Assert.AreEqual(3, repo.Count);
        }

        [Test]
        public void Remove_ExistingItem_ItemRemoved()
        {
            var repo = builder.WithItemsCount(3).Build();
            var item = repo[0];

            var removed = repo.Remove(item);

            Assert.True(removed);
            Assert.False(repo.Contains(item));
            Assert.AreEqual(2, repo.Count);
        }

        [Test]
        public void Remove_NonexistendItem_DoesNotThrow()
        {
            var repo = builder.Build();

            var removed = repo.Remove(new PasswordItem());

            Assert.False(removed);
            Assert.AreEqual(0, repo.Count);
        }

        [Test]
        public void OpenExsistingRepositoryTest()
        {
            var origin = builder.WithItemsCount(5).WithPassword("test").Build();

            var opened = new RepositorBaseBuilder()
                .WithPassword("test")
                .FromStream(new MemoryStream(builder.Stream.ToArray()))
                .Build();

            CollectionAssert.AreEqual(origin, opened, new PasswordItemEqualityComparer());
        }

        [Test]
        public void AddRemove_OneHundredTimes_CountIsCorrect()
        {
            var repo = builder.Build();
            var counter = 0;

            for (int i = 0; i < 100; i++)
            {
                if (i % 3 == 1)
                {
                    repo.Remove(repo.First());
                    counter--;
                }
                else
                {
                    repo.Save(new PasswordItem { Site = "1", Login = "2", Password = "3" });
                    counter++;
                }
            }

            Assert.AreEqual(counter, repo.Count);

        }

        [Test]
        public void Ctor_WrongPassword_ExceptionThrown()
        {
            var repo = builder.WithPassword("correct").Build();
            var data = builder.Stream.ToArray();

            Assert.Throws<PasswordException>(() =>
                {
                    repo = new RepositorBaseBuilder().WithPassword("wrong")
                    .FromStream(new MemoryStream(data))
                    .Build();
                });
        }

        [Test]
        public void ChangePassword_NewPassword_PasswordChanged()
        {
            var repo = builder.WithPassword("old").WithItemsCount(1).Build();

            Assert.DoesNotThrow(() => repo.ChangePassword("old", "new"));

            //re-open with new password
            Assert.DoesNotThrow(() =>
            {
                new RepositorBaseBuilder()
                .WithPassword("new")
                .FromStream(new MemoryStream(builder.Stream.ToArray()))
                .Build();
            });
        }

        [Test]
        public void ChangePassword_IncorrectOldPassword_PasswordNotChanged()
        {
            var repo = builder.WithPassword("old").WithItemsCount(1).Build();

            Assert.Throws<PasswordException>(() => repo.ChangePassword("wrong", "new"));

            //re-open with wrong new password
            Assert.Throws<PasswordException>(() =>
            {
                new RepositorBaseBuilder()
                .WithPassword("new")
                .FromStream(new MemoryStream(builder.Stream.ToArray()))
                .Build();
            });
        }
    }
}

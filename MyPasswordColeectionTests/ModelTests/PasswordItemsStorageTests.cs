using MPC.Model;
using MPC.Model.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPasswordColeectionTests.ModelTests
{
    [TestFixture]
    class PasswordItemsStorageTests
    {

        [Test]
        public void Append_OneItem_CountIncreace()
        {
            var storage = new PasswordItemsStorage(new MemoryStream(), new CryptoConverter(new FakeCrypter()));
            var originCount = storage.ReadAll().Length;

            storage.Append(new PasswordItem());
            var count = storage.ReadAll().Length;

            Assert.AreEqual(originCount + 1, count);
        }

        [Test]
        public void WriteAll_FiveItems_CountIsFive()
        {
            var storage = new PasswordItemsStorage(new MemoryStream(), new CryptoConverter(new FakeCrypter()));

            storage.WriteAll(Enumerable.Range(0,5).Select(x=> new PasswordItem()));
            var count = storage.ReadAll().Length;

            Assert.AreEqual(5, count);
        }

        [Test]
        public void WriteAll_LessItems_CountDecreace()
        {
            var storage = new PasswordItemsStorage(new MemoryStream(), new CryptoConverter(new FakeCrypter()));

            storage.WriteAll(Enumerable.Range(0, 5).Select(x => new PasswordItem()));
            storage.WriteAll(Enumerable.Range(0, 2).Select(x => new PasswordItem()));

            var count = storage.ReadAll().Length;

            Assert.AreEqual(2, count);
        }

        [Test]
        public void WriteAll_MoreItems_CountIncreace()
        {
            var storage = new PasswordItemsStorage(new MemoryStream(), new CryptoConverter(new FakeCrypter()));

            storage.WriteAll(Enumerable.Range(0, 2).Select(x => new PasswordItem()));
            storage.WriteAll(Enumerable.Range(0, 5).Select(x => new PasswordItem()));

            var count = storage.ReadAll().Length;

            Assert.AreEqual(5, count);
        }
    }
}
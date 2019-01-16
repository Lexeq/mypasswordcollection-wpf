using MPC.Model;
using MPC.Model.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPasswordColeectionTests.ModelTests
{
    [TestFixture]
    class PasswordItemConverterTests
    {
        [Test]
        public void Ctor_NullArg_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => new CryptoConverter(null));
        }

        [Test]
        public void Ctor_Valid_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new CryptoConverter(new FakeCrypter()));
        }

        [Test]
        public void FromBytes_ToBytes_Equals()
        {
            var converter = new CryptoConverter(new FakeCrypter());
            var item = new PasswordItem() { Site = "site", Password = "123" };

            var bytes = converter.ToBytes(item);
            var restoredItem = converter.FromBytes(bytes);

            Assert.True(new PasswordItemEqualityComparer().Equals(item, restoredItem));
        }

        [Test]
        public void ToBytes_NullArg_Throw()
        {
            var conv = new CryptoConverter(new FakeCrypter());

            Assert.Throws<ArgumentNullException>(() => conv.ToBytes(null));
        }

        [Test]
        public void FromBytes_NullArg_Throw()
        {
            var conv = new CryptoConverter(new FakeCrypter());

            Assert.Throws<ArgumentNullException>(() => conv.FromBytes(null));
        }

        [Test]
        public void ToBytes_InvalidArg_Throw()
        {
            var conv = new CryptoConverter(new FakeCrypter());

            Assert.Throws<ArgumentNullException>(() => conv.ToBytes(new PasswordItem { Site = "a", Login = "b", Password = null }));
        }

        [Test]
        public void FromBytes_InvalidArg_Throw()
        {
            var conv = new CryptoConverter(new FakeCrypter());
            var data = new byte[100];

            Assert.Throws<ArgumentException>(() => conv.FromBytes(data));
        }
    }
}

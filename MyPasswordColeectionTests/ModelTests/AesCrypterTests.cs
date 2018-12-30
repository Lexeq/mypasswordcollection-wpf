using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MPC.Model;
using MPC.Model.Repository;

namespace MyPasswordColeectionTests.ModelTests
{
    [TestFixture]
    class AesCrypterTests
    {
        AesCrypter aesCrypter;
        byte[] testData;

        [SetUp]
        public void Init()
        {
            aesCrypter = new AesCrypter("TestPassword");

            testData = new byte[256];
            new Random(1234).NextBytes(testData);
        }

        [Test]
        public void ThrowIfPasswordIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AesCrypter(null));
        }

        [Test]
        public void ThrowIfPasswordIsEmpty()
        {
            Assert.Throws<ArgumentException>(() => new AesCrypter(string.Empty));
        }

        [Test]
        public void ThrowIfEncryptNull()
        {
            Assert.Throws<ArgumentNullException>(() => aesCrypter.Encrypt(null));
        }

        [Test]
        public void ThrowIfDecryptNull()
        {
            Assert.Throws<ArgumentNullException>(() => aesCrypter.Decrypt(null));
        }

        [Test]
        public void EncryptTest()
        {
            Assert.DoesNotThrow(() => aesCrypter.Encrypt(testData));
        }

        [Test]
        public void EncryptDecryptTest()
        {
            var encData = aesCrypter.Encrypt(testData);
            var decData = aesCrypter.Decrypt(encData);

            CollectionAssert.AreEqual(testData, decData);
        }

        [Test]
        public void EncryptDecryptEmptyDataTest()
        {
            var encData = aesCrypter.Encrypt(testData);
            var decData = aesCrypter.Decrypt(encData);

            CollectionAssert.AreEqual(testData, decData);
        }
    }
}

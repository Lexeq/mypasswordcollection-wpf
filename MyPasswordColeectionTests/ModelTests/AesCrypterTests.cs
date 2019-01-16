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

            testData = new byte[2560];
            new Random(12345).NextBytes(testData);
        }

        [Test]
        public void Ctor_NullArg_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => new AesCrypter(null));
        }

        [Test]
        public void Ctor_EmptyPassword_Throw()
        {
            Assert.Throws<ArgumentException>(() => new AesCrypter(string.Empty));
        }

        [Test]
        public void Encrypt_NullArg_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => aesCrypter.Encrypt(null));
        }

        [Test]
        public void Decrypt_NullArg_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => aesCrypter.Decrypt(null));
        }

        [Test]
        public void Encrypt_Success()
        {
            Assert.DoesNotThrow(() => aesCrypter.Encrypt(testData));
        }

        [Test]
        public void Decrypt_EncryptedData_SameData()
        {
            var encData = aesCrypter.Encrypt(testData);
            var decData = aesCrypter.Decrypt(encData);

            CollectionAssert.AreEqual(testData, decData);
        }

        [Test]
        public void Decrypt_EncryptedEmptyData_SameData()
        {
            var data = Array.Empty<byte>();

            var encData = aesCrypter.Encrypt(data);
            var decData = aesCrypter.Decrypt(encData);

            CollectionAssert.AreEqual(data, decData);
        }

        [Test]
        public void Decrypt_NewCrypter_Sucssess()
        {
            var encData = aesCrypter.Encrypt(testData);

            var cr2 = new AesCrypter("TestPassword");
            var decData = cr2.Decrypt(encData);

            CollectionAssert.AreEqual(testData, decData);
        }
    }
}

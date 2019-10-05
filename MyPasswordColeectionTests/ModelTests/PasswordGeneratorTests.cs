using MPC.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPasswordColeectionTests.ModelTests
{
    class PasswordGeneratorTests
    {
        [Test]
        public void GeneratePassword()
        {
            int passwordLength = 14;
            PasswordGenerator gen = new PasswordGenerator();
            var password = gen.Generate(CharSet.Digits, passwordLength);

            Assert.AreEqual(passwordLength, password.Length);
            Assert.IsTrue(password.All(c => char.IsDigit(c)));        
        }
        [Test]
        public void GenerateOnlyLetters()
        {
            int passwordLength = 64;
            PasswordGenerator gen = new PasswordGenerator();
            var password = gen.Generate(CharSet.AllLetters, passwordLength);

            Assert.AreEqual(passwordLength, password.Length);
            Assert.IsTrue(password.All(c => char.IsLetter(c)));
        }
    }
}

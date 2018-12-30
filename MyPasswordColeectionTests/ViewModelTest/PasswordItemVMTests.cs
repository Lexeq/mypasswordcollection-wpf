using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPC.ViewModels;
using MPC.Model;

namespace MyPasswordColeectionTests.ViewModelTest
{
    [TestFixture]
    class PasswordItemVMTests
    {
        [Test]
        public void AcceptChangesTest()
        {
            string oldLogin = "login";
            string newLogin = "edited_login";
            PasswordItem password = new PasswordItem { Login = oldLogin };
            PasswordItemViewModel vm = new PasswordItemViewModel(password);

            vm.Login = newLogin;
            vm.AcceptChanges();

            Assert.AreEqual(newLogin, vm.Item.Login);
        }

        [Test]
        public void DeclineChangesTest()
        {
            string oldLogin = "login";
            string newLogin = "edited_login";
            PasswordItem password = new PasswordItem { Login = oldLogin };
            PasswordItemViewModel vm = new PasswordItemViewModel(password);

            vm.Login = newLogin;
            vm.DeclineChanges();

            Assert.AreEqual(oldLogin, vm.Item.Login);
        }

        [Test]
        public void ThrowIfNullArgumentTest()
        {
            Assert.Throws<ArgumentNullException>(() => new PasswordItemViewModel(null));
        }
    }
}
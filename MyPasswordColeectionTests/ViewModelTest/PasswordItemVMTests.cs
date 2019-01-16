﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPC.ViewModels;
using MPC.Model;
using System.Windows;

namespace MyPasswordColeectionTests.ViewModelTest
{
    [TestFixture]
    class PasswordItemVMTests
    {
        [Test]
        public void Accept_NewLogin_LoginChangrs()
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
        public void Decline_NewLogin_LoginNotChanged()
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
        public void Ctor_NullArg_Throw()
        {
            Assert.Throws<ArgumentNullException>(() => new PasswordItemViewModel(null));
        }

        [Test, RequiresThread(System.Threading.ApartmentState.STA)]
        public void CopyCommand_Text_TextInClipboard()
        {
            string site = "";
            PasswordItemViewModel vm = new PasswordItemViewModel(new PasswordItem { Site = site });

            vm.CopyCommand.Execute(vm.Site);

            Assert.AreEqual(site, Clipboard.GetText());
        }
    }
}
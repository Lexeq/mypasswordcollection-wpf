using MPC.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPasswordColeectionTests.ViewModelTest
{
    class PasswordGenerationViewModelTests
    {
        [Test]
        public void ErrorIfNoSetChoosen()
        {
            var vm = new PasswordGenerationViewModel();
            vm.UseDigits = vm.UseLetters = vm.UseSymbols = false;

            Assert.IsTrue(vm.HasErrors);
            Assert.Throws<ArgumentException>(() => vm.GenerateCommand.Execute(null));
        }

        [Test]
        public void PasswordLenghtTest([Values(10, 5, 1, 99)]int lenght)
        {
            var vm = new PasswordGenerationViewModel();
            vm.PasswordLength = lenght;
            vm.GenerateCommand.Execute(null);

            Assert.AreEqual(lenght, vm.GeneratedPassword.Length);
        }
    }
}

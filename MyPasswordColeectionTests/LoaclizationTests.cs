using MPC.ViewModels;
using NUnit.Framework;
using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace MyPasswordColeectionTests
{
    [TestFixture, RequiresThread(ApartmentState.STA)]
    class LoaclizationTests
    {
        [Test, TestCase("ru-ru"), TestCase("en-us")]
        public void DictionaryContainsAllUIStings(string culture)
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            dictionary.Source = new Uri($"pack://application:,,,/MyPasswordCollection;component/Resources/lang.{culture}.xaml", UriKind.Absolute);
            TextService localized = new TextService(dictionary);
            TextService nonlocalized = new TextService(new ResourceDictionary());
            foreach (UIStrings item in Enum.GetValues(typeof(UIStrings)))
            {
                Assert.AreNotEqual(nonlocalized.GetString(item), localized.GetString(item));
            }
        }

        [Test]
        public void DictionariesHaveSameNumberOfKeys()
        {
            var enus = new ResourceDictionary() { Source = new Uri($"pack://application:,,,/MyPasswordCollection;component/Resources/lang.en-us.xaml", UriKind.Absolute) };
            var ruru = new ResourceDictionary() { Source = new Uri($"pack://application:,,,/MyPasswordCollection;component/Resources/lang.ru-ru.xaml", UriKind.Absolute) };

            CollectionAssert.AreEquivalent(enus.Keys, ruru.Keys);
        }

        [SetUp]
        protected void FixtureSetUp()
        {
            new FrameworkElement();
        }
    }
}

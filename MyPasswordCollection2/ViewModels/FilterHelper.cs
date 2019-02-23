using MPC.Model;
using System;
using System.Collections;
using System.Globalization;

namespace MPC.ViewModels
{
    internal class FilterHelper : IComparer
    {
        public string Key { get; set; }

        public bool Filter(object obj)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf((obj as PasswordItem).Site, Key, CompareOptions.IgnoreCase) >= 0;
        }

        public FilterHelper(string key)
        {
            this.Key = key;
        }

        public int Compare(object x, object y)
        {
            var xs = ((PasswordItem)x).Site.StartsWith(Key, StringComparison.CurrentCultureIgnoreCase);
            var ys = ((PasswordItem)y).Site.StartsWith(Key, StringComparison.CurrentCultureIgnoreCase);

            if (xs == ys)
                return 0;
            if (xs)
                return -1;
            else
                return 1;
        }
    }
}



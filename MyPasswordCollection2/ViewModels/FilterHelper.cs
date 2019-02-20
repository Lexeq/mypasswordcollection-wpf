using System;
using System.Collections;
using System.Globalization;

namespace MPC.ViewModels
{
    internal class FilterHelper : IComparer
    {
        private readonly string key;

        public bool Filter(object obj)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf((obj as PasswordItemViewModel).Site, key, CompareOptions.IgnoreCase) >= 0;
        }

        public FilterHelper(string key)
        {
            this.key = key;
        }

        public int Compare(object x, object y)
        {
            var xs = ((PasswordItemViewModel)x).Site.StartsWith(key, StringComparison.CurrentCultureIgnoreCase);
            var ys = ((PasswordItemViewModel)y).Site.StartsWith(key, StringComparison.CurrentCultureIgnoreCase);

            if (xs == ys)
                return 0;
            if (xs)
                return -1;
            else
                return 1;
        }
    }
}



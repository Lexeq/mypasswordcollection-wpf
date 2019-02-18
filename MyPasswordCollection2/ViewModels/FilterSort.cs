using System;
using System.Collections;

namespace MPC.ViewModels
{
    internal class FilterSort : IComparer
    {
        private readonly string key;

        public FilterSort(string key)
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



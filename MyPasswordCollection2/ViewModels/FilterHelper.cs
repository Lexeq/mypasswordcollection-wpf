using MPC.Model;
using System;
using System.Collections;
using System.Globalization;

namespace MPC.ViewModels
{
    internal class FilterHelper : IComparer
    {
        public string FilterString { get; set; }

        public IList Source { get; }

        public bool PassesFilter(object obj)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf((obj as PasswordItem).Site, FilterString, CompareOptions.IgnoreCase) >= 0;
        }

        public FilterHelper(IList source)
        {
            Source = source;
            FilterString = string.Empty;
        }

        public int Compare(object x, object y)
        {
            if (string.IsNullOrEmpty(FilterString))
                return ComparePositions(x, y);

            var xStartsWithFilterString = ((PasswordItem)x).Site.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase);
            var yStartsWithFilterString = ((PasswordItem)y).Site.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase);

            if(xStartsWithFilterString && yStartsWithFilterString)
            {
                var comparisonOfLength = ((PasswordItem)x).Site.Length.CompareTo(((PasswordItem)y).Site.Length);
                return comparisonOfLength != 0 ? comparisonOfLength : ComparePositions(x, y);
            }
            else if(!xStartsWithFilterString && !yStartsWithFilterString)
            {
                return ComparePositions(x, y);
            }
            else
            {
                return xStartsWithFilterString ? -1 : 1;
            }
        }

        private int ComparePositions(object x, object y)
        {
            return Source.IndexOf((PasswordItem)x).CompareTo(Source.IndexOf((PasswordItem)y));
        }
    }
}



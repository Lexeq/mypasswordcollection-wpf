using MPC.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPasswordColeectionTests.ModelTests
{
    class PasswordItemEqualityComparer : IComparer, IComparer<PasswordItem>, IEqualityComparer<PasswordItem>
    {
        public int Compare(PasswordItem x, PasswordItem y)
        {
            return (x.Site == y.Site
                    && x.Login == x.Login
                    && x.Password == y.Password) ? 0 : -1;
        }

        public int Compare(object x, object y)
        {
            var a = x as PasswordItem;
            var b = y as PasswordItem;
            if (a == null || b == null)
                return 1;
            return Compare(a, b);
        }

        public bool Equals(PasswordItem x, PasswordItem y)
        {
            return x.Site == y.Site
                && x.Login == y.Login
                && x.Password == y.Password;
        }

        public int GetHashCode(PasswordItem obj)
        {
            int hash = 42;
            if (obj.Site != null)
                hash ^= obj.Site.GetHashCode();
            if (obj.Login != null)
                hash ^= obj.Login.GetHashCode();
            if (obj.Password != null)
                hash ^= obj.Password.GetHashCode();
            return hash;
        }
    }
}

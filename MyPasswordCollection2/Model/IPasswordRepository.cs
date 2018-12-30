using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Model
{
    public interface IPasswordRepository : IEnumerable<PasswordItem>, IDisposable
    {
        int Count { get; }

        PasswordItem this[int index]
        {
            get;
        }

        void Save(PasswordItem item);

        bool Remove(PasswordItem item);

        void Clear();
    }
}

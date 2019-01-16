using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Model.Repository
{
    interface IPasswordConverter
    {
        PasswordItem FromBytes(byte[] bytes);

        byte[] ToBytes(PasswordItem item);
    }
}

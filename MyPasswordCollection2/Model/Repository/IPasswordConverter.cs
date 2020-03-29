using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Model.Repository
{
    interface IPasswordConverter
    {
        /// <summary>
        /// Transform byte array to PasswordItem.
        /// </summary>
        PasswordItem FromBytes(byte[] bytes);

        /// <summary>
        /// Transform PasswordItem to byte array.
        /// </summary>
        byte[] ToBytes(PasswordItem item);
    }
}

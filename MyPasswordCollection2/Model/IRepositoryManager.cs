using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Model
{
    interface IRepositoryManager
    {
        IPasswordRepository GetRepository(string path, string password);
    }
}

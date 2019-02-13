using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Model
{
    public interface IRepositoryManager
    {
        IPasswordRepository Get(string path, string password);

        IPasswordRepository Create(string path, string password);

        void DeleteRepository(IPasswordRepository repository);
    }
}

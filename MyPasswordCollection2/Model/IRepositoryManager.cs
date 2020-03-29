using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Model
{
    public interface IRepositoryManager
    {
        /// <summary>
        /// Open a repository using given path.
        /// </summary>
        IPasswordRepository Open(string path, string password);

        /// <summary>
        /// Create new repository.
        /// </summary>
        IPasswordRepository Create(string path, string password);

        /// <summary>
        /// Delete a repository.
        /// </summary>
        /// <param name="repository">Repository to delete.</param>
        void DeleteRepository(IPasswordRepository repository);
    }
}

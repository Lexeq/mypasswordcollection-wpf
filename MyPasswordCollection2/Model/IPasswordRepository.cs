using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPC.Model
{
    public interface IPasswordRepository : IEnumerable<PasswordItem>, IDisposable
    {
        /// <summary>
        /// Get the number of elements contained in repository.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        PasswordItem this[int index]
        {
            get;
        }

        /// <summary>
        /// Add a item in repository if it doesn't contains this item or update an existing one.
        /// </summary>
        void Save(PasswordItem item);

        /// <summary>
        /// Remove specified item from the repository.
        /// </summary>
        bool Remove(PasswordItem item);

        /// <summary>
        /// Remove all items from the repository.
        /// </summary>
        void Clear();

        /// <summary>
        /// Change repository password.
        /// </summary>
        void ChangePassword(string oldPassword, string newPassword);
    }
}

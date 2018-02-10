using System.Collections.Generic;

namespace PasswordStorage
{
    interface IPasswordsCrypter
    {
        byte[] Encrypt(IEnumerable<PasswordItem> items, string password);

        PasswordItem[] Decrypt(byte[] bytes, string password);
    }
}

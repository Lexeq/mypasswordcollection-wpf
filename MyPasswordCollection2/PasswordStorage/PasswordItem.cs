using System;

namespace PasswordStorage
{
    public class PasswordItem
    {
        public string Site { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public PasswordItem() : this(string.Empty, string.Empty, string.Empty)
        { }

        public PasswordItem(string site, string login, string password)
        {
            if (site == null)
                throw new ArgumentNullException("String is null", nameof(site));
            if (login == null)
                throw new ArgumentNullException("String is null", nameof(login));
            if (password == null)
                throw new ArgumentNullException("String is null", nameof(password));

            Site = site;
            Login = login;
            Password = password;
        }

        public override string ToString()
        {
            return $"{Site} password";
        }
    }
}

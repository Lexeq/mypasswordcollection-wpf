using System;

namespace PasswordStorage
{
    public class PasswordItem
    {
        public string Site { get; private set; }

        public string Login { get; private set; }

        public string Password { get; private set; }

        public PasswordItem(string site, string login, string password)
        {
            if (string.IsNullOrEmpty(site))
                throw new ArgumentException("String is null or empty", nameof(site));
            if (string.IsNullOrEmpty(login))
                throw new ArgumentException("String is null or empty", nameof(login));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("String is null or empty", nameof(password));

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

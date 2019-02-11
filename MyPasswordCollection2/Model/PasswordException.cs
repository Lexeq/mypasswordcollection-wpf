using System;

namespace MPC.Model
{
    [Serializable]
    class PasswordException : Exception
    {
        public PasswordException() { }

        public PasswordException(string message)
            : base(message) { }

        public PasswordException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}

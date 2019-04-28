using MPC.Model;
using MPC.Model.Repository;
using System;

namespace MyPasswordColeectionTests.ModelTests
{
    class FakeCrypter : ICrypter
    {
        public byte[] Decrypt(byte[] data)
        {
            return data;
        }

        public byte[] Encrypt(byte[] data)
        {
            return data;
        }
    }
}

namespace MPC.Model
{
    interface ICrypter
    {
        byte[] Encrypt(byte[] data);

        byte[] Decrypt(byte[] data);
    }
}

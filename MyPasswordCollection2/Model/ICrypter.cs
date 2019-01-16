namespace MPC.Model
{
    public interface ICrypter
    {
        byte[] Encrypt(byte[] data);

        byte[] Decrypt(byte[] data);
    }
}

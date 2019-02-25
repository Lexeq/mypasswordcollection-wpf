namespace MPC.Model.Repository
{
    public interface ICrypter
    {
        byte[] Encrypt(byte[] data);

        byte[] Decrypt(byte[] data);
    }
}

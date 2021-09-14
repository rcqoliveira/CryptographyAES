namespace RC.CryptoAES.Api
{
    public interface ICryptography
    {
        string Encrypt(string text);
        string Decrypt(string text);
    }
}

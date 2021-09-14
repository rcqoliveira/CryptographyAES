using System;
using System.Security.Cryptography;
using System.Text;

namespace RC.CryptoAES.Api
{
    public class CryptoAES
    {
        public string Mode { get; set; }
        public string Key { get; set; }
        public int KeySize { get; set; }
        public int BlockSize { get; set; }
        public string Padding { get; set; }

        public RijndaelManaged ConfiguradeRijndaelManaged()
        {
            var encoding = Encoding.UTF8;

            return new RijndaelManaged
            {
                KeySize = this.KeySize,
                BlockSize = this.BlockSize,
                Padding = EnumParse<PaddingMode>(this.Padding),
                Mode = EnumParse<CipherMode>(this.Mode),
                Key = encoding.GetBytes(this.Key)
            };
        }

        private T EnumParse<T>(string text) 
        {
            return (T)Enum.Parse(typeof(T), text);
        }
    }

}

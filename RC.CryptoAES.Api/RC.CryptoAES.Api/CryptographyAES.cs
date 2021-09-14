using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace RC.CryptoAES.Api
{
    public class CryptographyAES : ICryptography
    {
        private readonly Encoding encoding = Encoding.UTF8;
        private readonly RijndaelManaged crypto;
        private readonly CryptoAES cryptoAES;

        public CryptographyAES(CryptoAES cryptoAES)
        {
            this.cryptoAES = cryptoAES;
            this.crypto = this.cryptoAES.ConfiguradeRijndaelManaged();
        }

        public string Decrypt(string text)
        {

            byte[] base64Decoded = Convert.FromBase64String(text);
            string base64DecodedStr = encoding.GetString(base64Decoded);

            var payload = JsonSerializer.Deserialize<Dictionary<string, string>>(base64DecodedStr);

            this.crypto.IV = Convert.FromBase64String(payload["iv"]);

            ICryptoTransform AESDecrypt = this.crypto.CreateDecryptor(this.crypto.Key, this.crypto.IV);
            byte[] buffer = Convert.FromBase64String(payload["value"]);

            return encoding.GetString(AESDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public string Encrypt(string text)
        {
            this.crypto.GenerateIV();

            ICryptoTransform AESEncrypt = this.crypto.CreateEncryptor(this.crypto.Key, this.crypto.IV);
            byte[] buffer = encoding.GetBytes(text);

            string encryptedText = Convert.ToBase64String(AESEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            string mac = BitConverter.ToString(HmacSHA256(Convert.ToBase64String(this.crypto.IV) + encryptedText, this.cryptoAES.Key)).Replace("-", "").ToLower();

            var keyValues = new Dictionary<string, object>
                {
                    { "iv", Convert.ToBase64String(this.crypto.IV) },
                    { "value", encryptedText },
                    { "mac", mac },
                };

            return Convert.ToBase64String(encoding.GetBytes(JsonSerializer.Serialize(keyValues)));
        }

        private byte[] HmacSHA256(String data, String key)
        {
            using (HMACSHA256 hmac = new HMACSHA256(encoding.GetBytes(key)))
            {
                return hmac.ComputeHash(encoding.GetBytes(data));
            }
        }
    }
}
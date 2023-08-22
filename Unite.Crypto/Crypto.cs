using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Web;


namespace Unite.Crypto
{
    public class Crypto
    {
        private static string cryptoKey;
        private static byte[] keyBytes;
        private static byte[] iv;

        public Crypto()
        {
            cryptoKey = "$UNITE$#@^@1CARE";
            keyBytes = Encoding.UTF8.GetBytes(cryptoKey);
            iv = Encoding.UTF8.GetBytes(cryptoKey);
        }

        public string Encrypt(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(cryptoKey)));
        }

        public string EncryptEncodeUrl(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            return HttpUtility.UrlEncode(Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(cryptoKey))));
        }

        public string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(cryptoKey)));
        }

        public string DecryptEncodeUrl(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(HttpUtility.UrlDecode(encryptedText))));
            return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(cryptoKey)));
        }
        private byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            byte[] transformFinalBlock = rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return transformFinalBlock;
        }

        private byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }

        private RijndaelManaged GetRijndaelManaged(string key)
        {
            var keyBytes = new byte[16];
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(key);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            var rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            rijndaelManaged.Padding = PaddingMode.PKCS7;
            rijndaelManaged.KeySize = 128;
            rijndaelManaged.BlockSize = 128;
            rijndaelManaged.Key = keyBytes;
            rijndaelManaged.IV = keyBytes;
            return rijndaelManaged;
        }
    }
}

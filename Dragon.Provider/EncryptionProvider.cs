using System.Security.Cryptography;
using System.Text;

namespace Dragon.Provider
{
    public static class EncryptionProvider
    {
        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key = Encoding.UTF8.GetBytes(ConfigProvider.EncryptionKey);
                encryptor.IV = new byte[16];
                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(clearBytes, 0, clearBytes.Length); cryptoStream.Close();
                clearText = Convert.ToBase64String(memoryStream.ToArray());
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                encryptor.Key = Encoding.UTF8.GetBytes(ConfigProvider.EncryptionKey);
                encryptor.IV = new byte[16];
                using MemoryStream memoryStream = new();
                CryptoStream cryptoStream = new(memoryStream, encryptor.CreateDecryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length); cryptoStream.Close();
                cipherText = Encoding.Unicode.GetString(memoryStream.ToArray());
            }
            return cipherText;
        }
        public static string StringToHex(string input)
        {
            byte[] stringBytes = Encoding.Unicode.GetBytes(input);
            StringBuilder sbBytes = new(stringBytes.Length * 2);
            foreach (byte b in stringBytes) { sbBytes.AppendFormat("{0:X2}", b); }
            return sbBytes.ToString();
        }
        public static string HexToString(string hexInput)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2) { bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16); }
            return Encoding.Unicode.GetString(bytes);
        }
    }
}

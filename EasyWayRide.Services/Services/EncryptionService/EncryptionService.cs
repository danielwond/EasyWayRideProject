using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace EasyWayRide.Services.Services.EncryptionService
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IConfiguration _options;

        public EncryptionService(IConfiguration options)
        {
            _options = options;
        }
        public string DecryptString(string type, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();
            var key = _options[$"Encryption:{type}"];
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream memoryStream = new(buffer);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);
            return streamReader.ReadToEnd();
        }

        public string EncryptString(string type, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;


            using (Aes aes = Aes.Create())
            {
                var key = _options[$"Encryption:{type}"];

                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
    }
}

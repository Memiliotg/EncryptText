using Application.Interfaces;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class AesEncryptionService : IEncryptionService
    {
        private readonly IKeyManagementService _keyManagementService;

       
        public AesEncryptionService(IKeyManagementService keyManagementService)
        {
            _keyManagementService = keyManagementService;
        }

        public string Encrypt(string plainText, out string hmac)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                hmac = string.Empty;
                return plainText;
            }

            
            var keyEntity = _keyManagementService.GetOrCreateCurrentKeyAsync().Result;
            byte[] key = Convert.FromBase64String(keyEntity.Key);
            byte[] iv = Convert.FromBase64String(keyEntity.IV);

            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            byte[] encrypted;

            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }
           
            using var hmacSha256 = new HMACSHA256(key);
            byte[] computedHmac = hmacSha256.ComputeHash(encrypted);
            hmac = Convert.ToBase64String(computedHmac);
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string cipherText, string hmac)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            var keyEntity = _keyManagementService.GetOrCreateCurrentKeyAsync().Result;
            byte[] key = Convert.FromBase64String(keyEntity.Key);
            byte[] iv = Convert.FromBase64String(keyEntity.IV);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            
            using var hmacSha256 = new HMACSHA256(key);
            var computedHmac = hmacSha256.ComputeHash(cipherBytes);
            if (Convert.ToBase64String(computedHmac) != hmac)
            {
                
                return "Advertencia: El texto cifrado fue alterado.";
            }

            
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(cipherBytes);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();  
        }

    }
}

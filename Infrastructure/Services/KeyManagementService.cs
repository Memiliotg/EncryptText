using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class KeyManagementService : IKeyManagementService
    {
        private readonly IEncryptionKeyRepository _keyRepository;
        public KeyManagementService(IEncryptionKeyRepository keyRepository)
        {
            _keyRepository = keyRepository;
        }

        public async Task<EncryptionKey> GetOrCreateCurrentKeyAsync()
        {
            var currentKey = await _keyRepository.GetCurrentKeyAsync();
            if (currentKey == null || (currentKey.ExpiresAt.HasValue && currentKey.ExpiresAt < DateTime.UtcNow))
            {
                
                using Aes aesAlg = Aes.Create();
                aesAlg.GenerateKey();
                aesAlg.GenerateIV();

                currentKey = new EncryptionKey
                {
                    Key = Convert.ToBase64String(aesAlg.Key),
                    IV = Convert.ToBase64String(aesAlg.IV),
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddHours(12)  
                };
                await _keyRepository.AddKeyAsync(currentKey);
            }
            return currentKey;
        }
    }
}

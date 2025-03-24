using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IEncryptionKeyRepository
    {
        Task<EncryptionKey> GetCurrentKeyAsync();
        Task AddKeyAsync(EncryptionKey key);
    }
}

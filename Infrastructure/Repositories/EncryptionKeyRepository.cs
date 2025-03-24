using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class EncryptionKeyRepository : IEncryptionKeyRepository
    {
        private readonly DeviceDbContext _context;

        public EncryptionKeyRepository(DeviceDbContext context)
        {
            _context = context;
        }

        public async Task<EncryptionKey> GetCurrentKeyAsync()
        {
            return await _context.EncryptionKeys.OrderByDescending(k => k.CreatedAt).FirstOrDefaultAsync();
        }

        public async Task AddKeyAsync(EncryptionKey key)
        {
            _context.EncryptionKeys.Add(key);
            await _context.SaveChangesAsync();
        }
    }
}

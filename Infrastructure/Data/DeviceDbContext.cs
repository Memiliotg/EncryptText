using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DeviceDbContext : DbContext
    {
        public DeviceDbContext(DbContextOptions<DeviceDbContext> options) : base(options) { }
        public DbSet<Device> Devices { get; set; }
        public DbSet<EncryptionKey> EncryptionKeys { get; set; }
    }
}

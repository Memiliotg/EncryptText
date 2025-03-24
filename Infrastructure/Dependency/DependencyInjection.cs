using Application.Interfaces;
using Application.UseCases;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repository;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Dependency
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DeviceDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IEncryptionKeyRepository, EncryptionKeyRepository>();
            services.AddScoped<IEncryptionService, AesEncryptionService>();
            services.AddScoped<IKeyManagementService, KeyManagementService>();

            return services;
        }
    }
}


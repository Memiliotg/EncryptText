using Application.Interfaces;
using Application.UseCases;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Dependency
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDeviceService, DeviceService>(); 
            return services;
        }
    }
}

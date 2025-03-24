
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;


namespace Application.UseCases
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly IEncryptionService _encryptionService;

        public DeviceService(IDeviceRepository deviceRepository, IEncryptionService encryptionService)
        {
            _deviceRepository = deviceRepository;
            _encryptionService = encryptionService;
        }

        public async Task<IEnumerable<DeviceDto>> GetAllDevicesAsync()
        {
            var devices = await _deviceRepository.GetAllAsync();
            return devices.Select(d => new DeviceDto
            {
                Id = d.Id,
                Name = d?.Name ?? "Desnocido",
                SensitiveData = !string.IsNullOrEmpty(d?.SensitiveData) && !string.IsNullOrEmpty(d?.DataHmac)
            ? _encryptionService.Decrypt(d.SensitiveData, d.DataHmac)
            : "No disponible"
            });
        }

        public async Task<DeviceDto> GetDeviceByIdAsync(int id)
        {
            var device = await _deviceRepository.GetByIdAsync(id);
            if (device == null) return null;
            return new DeviceDto
            {
                Id = device.Id,
                Name = device?.Name ?? "Desconocido",
                SensitiveData = !string.IsNullOrEmpty(device?.SensitiveData) && !string.IsNullOrEmpty(device?.DataHmac) ? _encryptionService.Decrypt(device.SensitiveData, device.DataHmac) : "No disponible"
            };
        }

        public async Task AddDeviceAsync(DeviceDto deviceDto)
        {
            string hmac;
            var encryptedData = _encryptionService.Encrypt(deviceDto.SensitiveData, out hmac);
            var device = new Device
            {
                Name = deviceDto.Name,
                SensitiveData = encryptedData,
                DataHmac = hmac
            };
            await _deviceRepository.AddAsync(device);
        }

        public async Task UpdateDeviceAsync(DeviceDto deviceDto)
        {
            var device = await _deviceRepository.GetByIdAsync(deviceDto.Id);
            if (device == null) return;
            string hmac;
            device.Name = deviceDto.Name;
            device.SensitiveData = _encryptionService.Encrypt(deviceDto.SensitiveData, out hmac);
            device.DataHmac = hmac;
            await _deviceRepository.UpdateAsync(device);
        }

        public async Task DeleteDeviceAsync(int id)
        {
            await _deviceRepository.DeleteAsync(id);
        }
    }
}

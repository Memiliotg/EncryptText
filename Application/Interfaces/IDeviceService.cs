using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDeviceService
    {
        Task<IEnumerable<DeviceDto>> GetAllDevicesAsync();
        Task<DeviceDto> GetDeviceByIdAsync(int id);
        Task AddDeviceAsync(DeviceDto deviceDto);
        Task UpdateDeviceAsync(DeviceDto deviceDto);
        Task DeleteDeviceAsync(int id);
    }
}

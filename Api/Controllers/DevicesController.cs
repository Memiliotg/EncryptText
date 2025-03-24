using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DevicesController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDto>>> GetDevices()
        {
            var devices = await _deviceService.GetAllDevicesAsync();
            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceDto>> GetDevice(int id)
        {
            var device = await _deviceService.GetDeviceByIdAsync(id);
            if (device == null)
                return NotFound();
            return Ok(device);
        }

        [HttpPost]
        public async Task<ActionResult> PostDevice([FromBody] DeviceDto deviceDto)
        {
            await _deviceService.AddDeviceAsync(deviceDto);
            return CreatedAtAction(nameof(GetDevice), new { id = deviceDto.Id }, deviceDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutDevice(int id, [FromBody] DeviceDto deviceDto)
        {
            if (id != deviceDto.Id)
                return BadRequest();
            await _deviceService.UpdateDeviceAsync(deviceDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDevice(int id)
        {
            await _deviceService.DeleteDeviceAsync(id);
            return NoContent();
        }
    }
}



using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Device
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required(ErrorMessage = "Los datos sensibles son obligatorios.")]
        public string? SensitiveData { get; set; }
        public string? DataHmac { get; set; }

    }
}

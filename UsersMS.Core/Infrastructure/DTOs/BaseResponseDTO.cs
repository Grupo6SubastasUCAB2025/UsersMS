using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UsersMS.Core.Infrastructure.DTOs
{
    public abstract class BaseResponseDTO
    {
        [Required]
        [JsonPropertyOrder(1)]
        public bool Success { get; set; }
        [Required]
        [JsonPropertyOrder(1)]
        public string Message { get; set; } = string.Empty;
        [Required]
        [JsonPropertyOrder(1)]
        public string UserEmail { get; set; } = string.Empty;
        [Required]
        [JsonPropertyOrder(1)]
        public DateTime Time { get; set; } = DateTime.UtcNow;
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UsersMS.Core.Infrastructure.DTOs
{
    public class BaseRequestDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [JsonPropertyOrder(1)]
        public string UserEmail { get; set; } = string.Empty;
    }
}

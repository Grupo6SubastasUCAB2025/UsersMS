using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Infrastructure.DTOs.Login
{
    public class LoginRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "Password is required.")]
        [JsonPropertyOrder(2)]
        public string Password { get; set; } = string.Empty;
    }
}

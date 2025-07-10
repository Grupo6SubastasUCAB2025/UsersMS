using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Auth.Infrastructure.DTOs.Logout
{
    public class LogoutRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "RefreshToken is required.")]
        [JsonPropertyOrder(2)]
        public string RefreshToken { get; set; } = string.Empty;
    }
}

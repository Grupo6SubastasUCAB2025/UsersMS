using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Infrastructure.DTOs.Logout
{
    public class LogoutResponseDTO : BaseResponseDTO
    {
        [Required]
        [JsonPropertyOrder(2)]
        public bool Active { get; set; }
    }
}

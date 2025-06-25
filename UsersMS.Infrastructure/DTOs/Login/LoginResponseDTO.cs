using System.Text.Json.Serialization;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Infrastructure.DTOs.Login
{
    public class LoginResponseDTO : BaseResponseDTO
    {

        [JsonPropertyOrder(2)]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyOrder(2)]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyOrder(2)]
        public string UserID { get; set; } = string.Empty;

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Auth.Infrastructure.DTOs.RefreshToken
{
    public class RefreshTokenResponseDTO : BaseResponseDTO
    {
        [Required]
        [JsonPropertyOrder(2)]
        public string AccessToken { get; set; } = string.Empty;
        [Required]
        [JsonPropertyOrder(2)]
        public string RefreshToken { get; set; } = string.Empty;
    }
}

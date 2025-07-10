using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Auth.Infrastructure.DTOs.Login
{
    public class LoginRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "Password is required.")]
        [JsonPropertyOrder(2)]
        public string Password { get; set; } = string.Empty;
    }
}

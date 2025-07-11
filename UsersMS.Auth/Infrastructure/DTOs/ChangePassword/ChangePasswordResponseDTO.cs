using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Auth.Infrastructure.DTOs.ChangePassword
{
    public class ChangePasswordResponseDTO : BaseResponseDTO
    {
        [Required]
        [JsonPropertyOrder(2)]
        public bool TemporaryPassword { get; set; }
    }
}

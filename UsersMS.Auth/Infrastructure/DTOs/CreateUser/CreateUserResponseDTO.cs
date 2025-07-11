using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Application.DTOs.Auth
{
    public class CreateUserResponseDTO : BaseResponseDTO
    {
        [Required(ErrorMessage = "User email is required.")]
        [JsonPropertyOrder(2)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role name is required.")]
        [JsonPropertyOrder(3)]
        public string NameRole { get; set; } = string.Empty;

        [JsonPropertyOrder(1)]
        public Guid UserId { get; set; } 
    }
}

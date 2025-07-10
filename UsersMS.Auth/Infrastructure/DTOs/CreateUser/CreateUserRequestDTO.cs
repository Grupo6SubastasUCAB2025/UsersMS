using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Application.DTOs.Auth
{
    public class CreateUserRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        [JsonPropertyOrder(2)]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [JsonPropertyOrder(2)]
        public string EmailToCreate { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role name is required.")]
        [JsonPropertyOrder(2)]
        public string NameRole { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [JsonPropertyOrder(2)]
        public string Password { get; set; } = string.Empty;
    }
}

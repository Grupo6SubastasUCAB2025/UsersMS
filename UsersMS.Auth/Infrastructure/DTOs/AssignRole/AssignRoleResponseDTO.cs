using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Application.DTOs.Auth
{
    public class AssignRoleResponseDTO : BaseResponseDTO
    {
        [Required(ErrorMessage = "User email is required.")]
        [JsonPropertyOrder(2)]
        public string UserEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required.")]
        [JsonPropertyOrder(2)]
        public string RoleName { get; set; } = string.Empty;
    }
}

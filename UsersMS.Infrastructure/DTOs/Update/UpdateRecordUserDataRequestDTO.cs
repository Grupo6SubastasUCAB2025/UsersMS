using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Infrastructure.DTOs.UpdateUser
{
    public class UpdateRecordUserDataRequestDTO : BaseRequestDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        [JsonPropertyOrder(1)]
        public required Guid UserId { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [JsonPropertyOrder(2)]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyOrder(3)]
        public string? Name { get; set; } = string.Empty;

        [JsonPropertyOrder(4)]
        public string? Phone { get; set; } = string.Empty;

        [JsonPropertyOrder(5)]
        public string? Cedula { get; set; } = string.Empty;

        [JsonPropertyOrder(6)]
        public string? BirthDate { get; set; } = string.Empty;

        [JsonPropertyOrder(12)]
        public string? Token { get; set; } = string.Empty;
    }
}

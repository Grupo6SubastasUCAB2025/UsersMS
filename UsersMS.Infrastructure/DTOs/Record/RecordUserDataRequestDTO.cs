using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UsersMS.Infrastructure.DTOs.RecordUserData
{
    public class RecordUserDataRequestDTO
    {
        [Required(ErrorMessage = "UserId is required.")]
        [JsonPropertyOrder(2)]
        public required Guid UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [JsonPropertyOrder(2)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [JsonPropertyOrder(2)]
        public required string UserEmail { get; set; }

        [Required(ErrorMessage = "Phone is required.")]
        [JsonPropertyOrder(2)]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Cedula is required.")]
        [JsonPropertyOrder(2)]
        public required string Cedula { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [JsonPropertyOrder(2)]
        public required string Role { get; set; } 

        [JsonPropertyOrder(2)]
        public string? Token { get; set; }
    }
}

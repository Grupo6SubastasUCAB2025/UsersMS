using System.ComponentModel.DataAnnotations;

namespace UsersMS.Infrastructure.DTOs.Email
{
    public class EmailRequestDTO
    {
        [Required(ErrorMessage = "ToEmail es requerido.")]
        public string ToEmail { get; set; } = string.Empty;
        [Required(ErrorMessage = "Subject es requerido.")]
        public string Subject { get; set; } = string.Empty;
        [Required(ErrorMessage = "Body es requerido.")]
        public string Body { get; set; } = string.Empty;
    }
}

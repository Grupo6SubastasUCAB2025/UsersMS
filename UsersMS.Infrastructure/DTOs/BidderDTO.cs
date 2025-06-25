using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Infrastructure.DTOs
{
    public class BidderDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string? Token { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
    }
}

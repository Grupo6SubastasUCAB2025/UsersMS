using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Infrastructure.DTOs
{
    public class GetAdministratorByNameResponseDTO
    {
        public List<AdministratorDTO> Administrators { get; set; } = new List<AdministratorDTO>();
    }
}

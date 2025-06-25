using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UsersMS.Core.Infrastructure.DTOs;

namespace UsersMS.Infrastructure.DTOs.Update
{
    public class UpdateRecordUserDataResponseDTO : BaseResponseDTO
    {
        [JsonPropertyOrder(2)]
        public Guid UserId { get; set; }
    }
}

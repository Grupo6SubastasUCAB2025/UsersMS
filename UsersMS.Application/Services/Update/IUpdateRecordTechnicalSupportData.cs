using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Infrastructure.DTOs.Update;
using UsersMS.Infrastructure.DTOs.UpdateUser;

namespace UsersMS.Application.Services.Update
{
    public interface IUpdateRecordTechnicalSupportData
    {
        Task<UpdateRecordUserDataResponseDTO> Execute(UpdateRecordUserDataRequestDTO request);
    }

}

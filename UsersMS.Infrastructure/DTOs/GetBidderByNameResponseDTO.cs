using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Infrastructure.DTOs
{
    public class GetBidderByNameResponseDTO
    {
        public List<BidderDTO> Bidders { get; set; }
    }
}

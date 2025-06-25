using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersMS.Infrastructure.DTOs
{
    public class GetAuctioneerByNameResponseDTO
    {
        public AuctioneerDTO? Auctioneer { get; set; }
        public List<AuctioneerDTO> Auctioneers { get; set; } = new();
    }
}

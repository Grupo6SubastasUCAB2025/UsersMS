using UsersMS.Domain.Entities;

namespace UsersMS.Domain.Repositories
{
    public interface IAuctioneerRepository
    {
        Task<List<Auctioneer>> GetAllAuctioneersAsync();
        Task<Auctioneer> GetByIdAsync(Guid id);
        Task<List<Auctioneer>> GetByNameAsync(string name);
        Task AddAuctioneerAsync(Auctioneer auctioneer);
        Task UpdateAuctioneerAsync(Auctioneer auctioneer);
    }
}

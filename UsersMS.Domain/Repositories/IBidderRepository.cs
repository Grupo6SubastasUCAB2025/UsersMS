using UsersMS.Domain.Entities;

public interface IBidderRepository
{
    Task<List<Bidder>> GetAllBiddersAsync();
    Task<Bidder> GetBidderByIdAsync(Guid id);
    Task<List<Bidder>> GetBiddersByNameAsync(string name);
    Task AddBidderAsync(Bidder bidder);
    Task UpdateBidderAsync(Bidder bidder);
}
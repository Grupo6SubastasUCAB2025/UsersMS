using Microsoft.EntityFrameworkCore;
using UsersMS.Domain.Entities;
using UsersMS.Domain.ValueObject;
using UsersMS.Infrastructure.Database;
using UsersMS.Domain.Repositories;

namespace UsersMS.Infrastructure.Repositories
{
    public class BidderRepository : IBidderRepository
    {
        private readonly UserDbContext _context;

        public BidderRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<List<Bidder>> GetAllBiddersAsync()
        {
            return await _context.Bidders.ToListAsync();
        }

        public async Task<Bidder> GetBidderByIdAsync(Guid id)
        {
            var bidder = await _context.Bidders.FindAsync(new UserId(id));
            if (bidder == null)
            {
                throw new KeyNotFoundException($"Bidder with ID {id} not found.");
            }

            return bidder;
        }

        public async Task<List<Bidder>> GetBiddersByNameAsync(string name)
        {
            return await _context.Bidders
                .Where(b => b.Name.Value.ToLower().Contains(name.ToLower()))
                .ToListAsync();
        }

        public async Task AddBidderAsync(Bidder bidder)
        {
            _context.Bidders.Add(bidder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBidderAsync(Bidder bidder)
        {
            var existing = await _context.Bidders.FindAsync(bidder.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Bidder with ID {bidder.Id} not found.");
            }

            existing.ChangeName(bidder.Name);
            existing.ChangePhone(bidder.Phone);
            existing.ChangeCedula(bidder.Cedula);

            await _context.SaveChangesAsync();
        }
    }
}

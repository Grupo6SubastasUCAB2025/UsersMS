using Microsoft.EntityFrameworkCore;
using UsersMS.Domain.Entities;
using UsersMS.Domain.ValueObject;
using UsersMS.Infrastructure.Database;
using UsersMS.Infrastructure.DTOs;
using UsersMS.Infrastructure.Mappers;
using UsersMS.Domain.Repositories;

namespace UsersMS.Infrastructure.Repositories
{
    public class AuctioneerRepository : IAuctioneerRepository
    {
        private readonly UserDbContext _context;

        public AuctioneerRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<List<Auctioneer>> GetAllAuctioneersAsync()
        {
            return await _context.Auctioneers.ToListAsync();
        }

        public async Task<Auctioneer> GetByIdAsync(Guid id)
        {
            var auctioneer = await _context.Auctioneers.FindAsync(new UserId(id));
            if (auctioneer == null)
            {
                throw new KeyNotFoundException($"Auctioneer with ID {id} not found.");
            }
            return auctioneer;
        }

        public async Task<List<Auctioneer>> GetByNameAsync(string name)
        {
            var auctioneers = await _context.Auctioneers
                .Where(a => a.Name.Value.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            if (!auctioneers.Any())
            {
                throw new KeyNotFoundException($"No auctioneers with name containing '{name}' found.");
            }

            return auctioneers;
        }

        public async Task AddAuctioneerAsync(Auctioneer auctioneer)
        {
            _context.Auctioneers.Add(auctioneer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAuctioneerAsync(Auctioneer auctioneer)
        {
            var existing = await _context.Auctioneers.FindAsync(auctioneer.Id);
            if (existing == null)
            {
                throw new KeyNotFoundException($"Auctioneer with ID {auctioneer.Id} not found.");
            }

            _context.Entry(existing).CurrentValues.SetValues(auctioneer);
            await _context.SaveChangesAsync();
        }
    }

}

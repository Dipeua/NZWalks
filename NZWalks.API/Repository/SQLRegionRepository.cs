using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repository
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly ApplicationDbContext _db;
        public SQLRegionRepository(ApplicationDbContext db)
        {
            _db = db;   
        }
        public async Task<List<Region>> GetAllAsync()
        {
            return await _db.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _db.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }


        public async Task<Region> CreateAsync(Region region)
        {
            await _db.Regions.AddAsync(region);
            await _db.SaveChangesAsync();
            return region;
        }
        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var exitingRegion = await _db.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (exitingRegion == null)
            {
                return null;
            }
            exitingRegion.Name = region.Name;
            exitingRegion.Code = region.Code;
            exitingRegion.RegionImageUrl = region.RegionImageUrl;
            await _db.SaveChangesAsync();
            return exitingRegion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var exitingRegion = await _db.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (exitingRegion == null)
            {
                return null;
            }

            _db.Remove(exitingRegion);
            await _db.SaveChangesAsync();
            return exitingRegion;
        }
        
    }
}

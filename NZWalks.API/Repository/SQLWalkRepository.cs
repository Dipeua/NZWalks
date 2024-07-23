using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository;

public class SQLWalkRepository : IWalkRepository
{
    private readonly ApplicationDbContext _db;
    public SQLWalkRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<Walk> CreateAsync(Walk walk)
    {
        await _db.Walks.AddAsync(walk);
        await _db.SaveChangesAsync();
        return walk;
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {
        var exitWalk = await _db.Walks.FirstOrDefaultAsync(w => w.Id == id);
        if(exitWalk == null) { return null; }

        _db.Walks.Remove(exitWalk);
        await _db.SaveChangesAsync();
        return exitWalk;
    }

    public async Task<List<Walk>> GetAllAsync()
    {
        return await _db.Walks
            .Include("Difficulty").Include("Region")
            .ToListAsync();
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await _db.Walks
            .Include("Difficulty").Include("Region")
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        var exitWalk = await _db.Walks.FirstOrDefaultAsync(w => w.Id == id);
        if (exitWalk == null) { return null; }

        exitWalk.Name = walk.Name;
        exitWalk.Description = walk.Description;
        exitWalk.LengthInKm = walk.LengthInKm;
        exitWalk.WalkImageUrl = walk.WalkImageUrl;

        exitWalk.DifficultyId = walk.DifficultyId;
        exitWalk.RegionId = walk.RegionId;

        await _db.SaveChangesAsync();
        return exitWalk;
    }
}

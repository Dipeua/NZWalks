using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Linq;

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

    public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool? isAscending = true, int? pageNumber = 1, int? pageSize = 1000)
    {
        var walks = _db.Walks.Include("Difficulty").Include("Region").AsQueryable();
        // Filtering
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if(filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(w => w.Name.Contains(filterQuery));
            }
        }

        //Sorting
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = (bool)isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
            }
            else if (sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
            {
                walks = (bool)isAscending ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
            }
        }

        // Pagination
        var skipResults = (pageNumber - 1) * pageSize;

        return await walks.Skip((int)skipResults).Take((int)pageSize).ToListAsync();
        //return await _db.Walks
        //    .Include("Difficulty").Include("Region")
        //    .ToListAsync();
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

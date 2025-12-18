using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;
        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null)
        {
             var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
             // Filtering
            if ( string.IsNullOrWhiteSpace(filterOn) == false &&
                 string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
               else if (filterOn.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    // Support range filter like "min-max" or exact value
                    if (filterQuery.Contains('-'))
                    {
                        var parts = filterQuery.Split('-', StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 1 && double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var min))
                        {
                            if (parts.Length >= 2 && double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var max))
                            {
                                walks = walks.Where(x => x.LengthInKm >= min && x.LengthInKm <= max);
                            }
                            else
                            {
                                walks = walks.Where(x => x.LengthInKm >= min);
                            }
                        }
                    }
                    else
                    {
                        if (double.TryParse(filterQuery, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                        {
                            walks = walks.Where(x => x.LengthInKm == value);
                        }
                    }
                }
            }
             return await walks.ToListAsync();
            // return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

       

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.WalkImageUrl = walk.WalkImageUrl;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
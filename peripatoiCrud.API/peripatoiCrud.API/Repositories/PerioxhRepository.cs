using Microsoft.EntityFrameworkCore;
using peripatoiCrud.API.Data;
using peripatoiCrud.API.Models.Domain;

namespace peripatoiCrud.API.Repositories
{
    public class PerioxhRepository : IPerioxhRepository
    {
        private readonly PeripatoiDbContext dbContext;

        public PerioxhRepository(PeripatoiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Perioxh> CreateAsync(Perioxh perioxh)
        {
            await dbContext.Perioxes.AddAsync(perioxh);
            await dbContext.SaveChangesAsync();
            return perioxh;
        }

        public async Task<List<Perioxh>> GetAllAsync()
        {
            return await dbContext.Perioxes.ToListAsync();
        }

        public async Task<Perioxh?> GetByIdAsync(Guid id)
        {
            return await dbContext.Perioxes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Perioxh?> UpdateAsync(Guid id, Perioxh perioxh)
        {
            var perioxhResult = await dbContext.Perioxes.FirstOrDefaultAsync(x => x.Id == id);

            if (perioxhResult == null)
            {
                return null;
            }

            perioxhResult.Kwdikos = perioxh.Kwdikos;
            perioxhResult.Onoma = perioxh.Onoma;
            perioxhResult.EikonaUrl = perioxh.EikonaUrl;

            await dbContext.SaveChangesAsync();
            return perioxhResult;
        }

        public async Task<Perioxh?> DeleteAsync(Guid id)
        {
            var perioxhResult = await dbContext.Perioxes.FirstOrDefaultAsync(x => x.Id == id);

            if (perioxhResult == null)
            {
                return null;
            }

            dbContext.Perioxes.Remove(perioxhResult);
            await dbContext.SaveChangesAsync();

            return perioxhResult;
        }
    }
}

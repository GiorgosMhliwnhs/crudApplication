using peripatoiCrud.API.Data;
using peripatoiCrud.API.Models.Domain;

namespace peripatoiCrud.API.Repositories
{
    public class PeripatosRepository : IPeripatosRepository
    {
        private readonly PeripatoiDbContext dbContext;

        public PeripatosRepository(PeripatoiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<Peripatos> CreateAsync(Peripatos peripatos)
        {
            await dbContext.Peripatoi.AddAsync(peripatos);
            await dbContext.SaveChangesAsync();

            return peripatos;
        }
    }
}

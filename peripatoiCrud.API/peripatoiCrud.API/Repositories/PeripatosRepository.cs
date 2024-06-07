using Microsoft.EntityFrameworkCore;
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

        public Task<Peripatos?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Peripatos>> GetAllAsync()
        {
            //εδω κανουμε χρηση του include απο το entity framewoek, το οποιο στην ουσια μας επιτρεπει να κανουμε get και τα 2 navigation properties
            //δυσκολια και περιοχη τα οποια εχουμε δηλωσει στην κλαση του περιπατου μεσω των id τα οποια εχουμε ορισει
            return await dbContext.Peripatoi.Include(x=>x.Dyskolia).Include(x=>x.Perioxh).ToListAsync(); 
        }

        public Task<Peripatos?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Peripatos?> UpdateAsync(Guid id, Peripatos peripatos)
        {
            throw new NotImplementedException();
        }
    }
}

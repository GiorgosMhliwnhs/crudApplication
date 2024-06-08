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

        public async Task<Peripatos?> DeleteAsync(Guid id)
        {
            var peripatosResult = await dbContext.Peripatoi.FirstOrDefaultAsync(x => x.Id == id);

            if (peripatosResult == null)
            {
                return null;
            }

            dbContext.Peripatoi.Remove(peripatosResult);
            await dbContext.SaveChangesAsync();

            return peripatosResult;
        }

        public async Task<List<Peripatos>> GetAllAsync()
        {
            //εδω κανουμε χρηση του include απο το entity framewoek, το οποιο στην ουσια μας επιτρεπει να κανουμε get και τα 2 navigation properties
            //δυσκολια και περιοχη τα οποια εχουμε δηλωσει στην κλαση του περιπατου μεσω των id τα οποια εχουμε ορισει
            return await dbContext.Peripatoi.Include(x=>x.Dyskolia).Include(x=>x.Perioxh).ToListAsync(); 
        }

        public async Task<Peripatos?> GetByIdAsync(Guid id)
        {
            return await dbContext.Peripatoi.Include(x=>x.Dyskolia).Include(x => x.Perioxh).FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task<Peripatos?> UpdateAsync(Guid id, Peripatos peripatos)
        {
            var peripatosResult = await dbContext.Peripatoi.FirstOrDefaultAsync(x => x.Id == id);

            if (peripatosResult == null)
            {
                return null;
            }

            peripatosResult.Onoma = peripatos.Onoma;
            peripatosResult.Perigrafh = peripatos.Perigrafh; 
            peripatosResult.Mhkos = peripatos.Mhkos;
            peripatosResult.EikonaUrl = peripatos.EikonaUrl;
            peripatosResult.DyskoliaId = peripatos.DyskoliaId;
            peripatosResult.PerioxhId = peripatos.PerioxhId;

            await dbContext.SaveChangesAsync();
            return peripatosResult;
        }
    }
}
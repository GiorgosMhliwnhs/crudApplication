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

        public async Task<List<Peripatos>> GetAllAsync(string? filter = null, string? filterQuery = null, string? sortBy = null, bool afksousa = true, int arithmosSelidwn = 1, int megethosSelidas = 1000)
        {
            //εδω κανουμε χρηση του include απο το entity framewoek, το οποιο στην ουσια μας επιτρεπει να κανουμε get και τα 2 navigation properties
            //δυσκολια και περιοχη τα οποια εχουμε δηλωσει στην κλαση του περιπατου μεσω των id τα οποια εχουμε ορισει 
            var peripatoi = dbContext.Peripatoi.Include(x => x.Dyskolia).Include(x => x.Perioxh).AsQueryable();
            
            //φιλτραρισμα -> ελεγχουμε οτι οι παραμετροι δεν ειναι αδειοι, και μετα ελεγχουμε εαν ο χρηστης χρησιμοποιησε φιλτρα για ονομα περιπατου
            // στην οποια περιπτωη επιστρεφουμε στο queryable ολες τις περιπτωσεις που περιεχουν το string που παρειχε ο χρηστης
            if (!string.IsNullOrWhiteSpace(filter) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filter.Equals("Onoma", StringComparison.OrdinalIgnoreCase))
                {
                    peripatoi = peripatoi.Where(x => x.Onoma.Contains(filterQuery));
                }
            }

            //Ταξινομηση -> ελεγχουμε οτι η παραμετρος ταξινομησης δεν ειναι αδεια, και εαν δεν ειναι ελεγχουμε την παραμετρο αυξουσας ταξινομησης
            // και αναλογως καλουμε την orderby(ascending-αυξουσα) ή την orderbydescending(φθινουσα). Ελεγχουμε με την if εαν προκειται για ταξινομηση ονοματος ή μηκους
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Onoma", StringComparison.OrdinalIgnoreCase))// ταξινομηση κατα ονομα
                {
                    peripatoi = afksousa ? peripatoi.OrderBy(x => x.Onoma) : peripatoi.OrderByDescending(x => x.Onoma);
                }
                else if (sortBy.Equals("Mhkos", StringComparison.OrdinalIgnoreCase)) // ταξινομηση κατα μηκος
                {
                    peripatoi = afksousa ? peripatoi.OrderBy(x => x.Mhkos) : peripatoi.OrderByDescending(x => x.Mhkos);

                }
            }

            //pagination -> υπολογιζουμε τον αριθμο αποτελεσματων που θα προσπεραστει πολλαπλασιαζοντας τον αριθμο σελιδων με τα αποτελεσματα που μπορει να εχει
            // το καθε page. Στην περιπτωση που η σελιδα ειναι μονο 1 τοτε μηδενιζεται η πραξη και δεν γινεται προσπεραση, ανταυτου ερχονται ολα τα δεδομενα.
            var apotelesmataGiaProsperash = (arithmosSelidwn - 1) * megethosSelidas;

            return await peripatoi.Skip(apotelesmataGiaProsperash).Take(megethosSelidas).ToListAsync();
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
using Microsoft.EntityFrameworkCore;
using peripatoiCrud.API.Models.Domain;

namespace peripatoiCrud.API.Data
{
    public class PeripatoiDbContext : DbContext
    {
        public PeripatoiDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        //εδω οριζουμε τα db set μας βαση των μοντελων
        public DbSet<Dyskolia> Dyskolies { get; set; }
        public DbSet<Perioxh> Perioxes { get; set; }
        public DbSet<Peripatos> Peripatoi { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace peripatoiCrud.API.Data
{
    public class PeripatoiAuthDbContext : IdentityDbContext
    {
        public PeripatoiAuthDbContext(DbContextOptions<PeripatoiAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readId = "4aeaa46a-084a-4493-89c9-dac944d88876"; // guid για read only ρολο, δηλαδη μονο για get operations
            var writeId = "4a5a04c7-eadc-4d9d-a24d-10a87d0dd759"; //guid για write ρολο, δηλαδη να μπορει να προσθετει, τροποποιει και να διαγραφει εγγραφες

            //δημιουργουμε τη λιστα με τους ρολους για την βαση 
            var roloi = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = readId,
                    ConcurrencyStamp = readId,
                    Name = "Read",
                    NormalizedName  = "Read".ToUpper()
                },
                new IdentityRole
                {
                    Id = writeId,
                    ConcurrencyStamp = writeId,
                    Name = "Write",
                    NormalizedName  = "Write".ToUpper()
                },
            };

            builder.Entity<IdentityRole>().HasData(roloi);
        }
    }
}

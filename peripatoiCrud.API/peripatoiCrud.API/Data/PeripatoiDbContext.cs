using Microsoft.EntityFrameworkCore;
using peripatoiCrud.API.Models.Domain;

namespace peripatoiCrud.API.Data
{
    public class PeripatoiDbContext : DbContext
    {
        public PeripatoiDbContext(DbContextOptions<PeripatoiDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        //εδω οριζουμε τα db set μας βαση των μοντελων
        public DbSet<Dyskolia> Dyskolies { get; set; }
        public DbSet<Perioxh> Perioxes { get; set; }
        public DbSet<Peripatos> Peripatoi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //με αυτο θα περασουμε στην βαση τις δυσκολιες με "ευκολια"
            var dyskolies = new List<Dyskolia>() // θα εχουμε για αρχη 3 δυσκολιες και μετα ισως προστεθουν κι αλλες, επισης χρησιμοποιω σε ολα λατινικους χαρακτηρες επειδη δυσκολευεται να καταλαβει ελληνικους
            {
                new Dyskolia()
                {
                    Id = Guid.Parse("09061773-9946-4c79-804e-0f33f6c23213"),
                    Onoma = "EYKOLOS"
                },
                new Dyskolia()
                {
                    Id = Guid.Parse("f4db66cf-1936-48ff-b8e7-8c99701bcfd9"),
                    Onoma = "METRIOS"
                },
                new Dyskolia()
                {
                    Id = Guid.Parse("ca2f6118-2f8f-4ca5-99f5-31287e3dcf15"),
                    Onoma = "DYSKOLOS"
                },
            };

            //τελος περναμε τις δυσκολιες στην βαση με το model builder χαρη στο EF 
            modelBuilder.Entity<Dyskolia>().HasData(dyskolies);

            //περναμε τα δεδομενα για τις περιοχες
            var perioxes = new List<Perioxh>()
            {
                new Perioxh()
                {
                    Id = Guid.Parse("858e610e-fa5e-47dc-9ec4-38af459e1646"),
                    Onoma = "ATHINA",
                    Kwdikos = "ATH",
                    EikonaUrl = "https://media.istockphoto.com/id/1028749698/photo/the-acropolis-of-athens-greece.jpg?s=2048x2048&w=is&k=20&c=FMjbIryuJtIGU83EaaBafEDW49KjhcnUA3TYHfKZWYs="
                },
                new Perioxh()
                {
                    Id = Guid.Parse("5a1cc102-9bbf-4818-aa76-52d7ebdcff21"),
                    Onoma = "THESSALONIKI",
                    Kwdikos = "THS",
                    EikonaUrl = "https://images.pexels.com/photos/17505177/pexels-photo-17505177/free-photo-of-white-tower-in-thessaloniki.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Perioxh()
                {
                    Id = Guid.Parse("d96fe13d-5c86-4425-8075-0c9d3e062d81"),
                    Onoma = "NAFPLIO",
                    Kwdikos = "NFP",
                    EikonaUrl = "https://images.pexels.com/photos/10400151/pexels-photo-10400151.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Perioxh()
                {
                    Id = Guid.Parse("79816b96-50ac-43eb-8b74-9407fb54c613"),
                    Onoma = "KRHTH",
                    Kwdikos = "CRE",
                    EikonaUrl = "https://images.pexels.com/photos/13861594/pexels-photo-13861594.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Perioxh()
                {
                    Id = Guid.Parse("367e8f8a-6a1b-4d44-b6b8-9366e21bbb83"),
                    Onoma = "RODOS",
                    Kwdikos = "RHO",
                    EikonaUrl = "https://images.pexels.com/photos/3327997/pexels-photo-3327997.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                }
            };

            //τελος περναμε τις περιοχες στην βαση με το model builder χαρη στο EF 
            modelBuilder.Entity<Perioxh>().HasData(perioxes);
        }
    }
}

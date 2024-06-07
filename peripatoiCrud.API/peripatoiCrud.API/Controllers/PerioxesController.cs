using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using peripatoiCrud.API.Data;
using peripatoiCrud.API.Models.Domain;
using peripatoiCrud.API.Models.DTOs;

namespace peripatoiCrud.API.Controllers
{
    [Route("api/perioxes")]
    [ApiController]
    public class PerioxesController : ControllerBase
    {
        private readonly PeripatoiDbContext dbContext;

        //κανυομε inject το dbcontext για να μπορεσουμε να το χρησιμοποιησουμε παρακατω
        public PerioxesController(PeripatoiDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //https://localhost:7229/api/perioxes
        //Ληψη ολων των περιοχων
        [HttpGet]
        public IActionResult GetAll()
        {
            var perioxesDomain = dbContext.Perioxes.ToList();

            var perioxesDto = new List<PerioxhDto>();
            //εδω κανουμε το map απο μοντελο σε dto, η αλλαγη εγινε γιατι ειναι best practice να εμφανιζουμε τα δεδομενα απο το dto παρα κατευθειαν τα μοντελα
            foreach (var perioxhDomain in perioxesDomain)
            {
                perioxesDto.Add(new PerioxhDto()
                {
                    Id = perioxhDomain.Id,
                    Onoma = perioxhDomain.Onoma,
                    Kwdikos = perioxhDomain.Kwdikos,
                    EikonaUrl = perioxhDomain.EikonaUrl
                });
            }

            return Ok(perioxesDto);
        }

        //https://localhost:7229/api/perioxes/{id}
        //Ληψη συγκεκριμενης περιοχης βαση του id της
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var perioxhDomain = dbContext.Perioxes.FirstOrDefault(x => x.Id == id);

            if (perioxhDomain == null) //ελεγχος εαν υπαρχει στη βαση, στην περιπτωση που δεν υπαρχει επιστρεφουμε 404
            {
                return NotFound();
            }

            //κανουμε εδω το μαπ και μετα επιστρεφουμε την περιοχη οπως στην get all
            var perioxhDto = new PerioxhDto()
            {
                Id = perioxhDomain.Id,
                Onoma = perioxhDomain.Onoma,
                Kwdikos = perioxhDomain.Kwdikos,
                EikonaUrl = perioxhDomain.EikonaUrl
            };

            return Ok(perioxhDto);
        }

        //https://localhost:7229/api/perioxes/
        //Δημιουργια περιοχης
        [HttpPost]
        public IActionResult Create([FromBody] AddPerioxhRequestDto addPerioxhRequestDto)
        {
            var perioxhDomainModel = new Perioxh
            {
                Kwdikos = addPerioxhRequestDto.Kwdikos,
                Onoma = addPerioxhRequestDto.Onoma,
                EikonaUrl = addPerioxhRequestDto.EikonaUrl
            };

            dbContext.Perioxes.Add(perioxhDomainModel);
            dbContext.SaveChanges();

            var perioxhDto = new PerioxhDto
            {
                Id = perioxhDomainModel.Id,
                Onoma = perioxhDomainModel.Onoma,
                Kwdikos = perioxhDomainModel.Kwdikos,
                EikonaUrl = perioxhDomainModel.EikonaUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = perioxhDomainModel.Id }, perioxhDto);
        }
    }
}

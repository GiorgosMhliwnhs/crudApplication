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

        //https://localhost:7229/api/perioxes/{id}
        //Επεξεργασια περιοχης
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdatePerioxhRequestDto updatePerioxhRequestDto)
        {
            // αρχικα ψαχνουμε μεσω του db context εαν υπαρχει περιοχη με το id που μας περασε ο χρηστης
            var perioxh = dbContext.Perioxes.FirstOrDefault(x => x.Id == id);


            if (perioxh == null)
            {
                return NotFound(); // εαν το dbcontext επιστρεψει στην περιοχη null τοτε και εμεις επιστρεφουμε στον χρηστη not found
            }

            //εαν βρεθει ομως τοτε κανουμε populate το μοντελο με τις τιμες που περασε ο χρηστης και σωζουμε
            perioxh.Kwdikos = updatePerioxhRequestDto.Kwdikos;
            perioxh.Onoma = updatePerioxhRequestDto.Onoma;
            perioxh.EikonaUrl = updatePerioxhRequestDto?.EikonaUrl;

            dbContext.SaveChanges();

            // τελος τα περναμε ολα στο dto και το στελνουμε πισω στον χρηστη με 200αρι
            var perioxhDto = new PerioxhDto
            {
                Id = perioxh.Id,
                Kwdikos = perioxh.Kwdikos,
                Onoma = perioxh.Onoma,
                EikonaUrl = perioxh.EikonaUrl
            };

            return Ok (perioxhDto);
        }
    }
}

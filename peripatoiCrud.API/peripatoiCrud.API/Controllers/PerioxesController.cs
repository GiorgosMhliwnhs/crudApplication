
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using peripatoiCrud.API.Data;
using peripatoiCrud.API.Models.Domain;
using peripatoiCrud.API.Models.DTOs;
using peripatoiCrud.API.Repositories;

namespace peripatoiCrud.API.Controllers
{
    [Route("api/perioxes")]
    [ApiController]
    public class PerioxesController : ControllerBase
    {
        private readonly PeripatoiDbContext dbContext;
        private readonly IPerioxhRepository perioxhRepository;

        //κανυομε inject το dbcontext για να μπορεσουμε να το χρησιμοποιησουμε παρακατω, μετα το work item #0018 περναμε πλεον και το interface του repository και αντι να χρησιμοποιουμε το 
        //dbcontext αφηνουμε την υλοποιηση του interface (PerioxhRepository) να κανει το operation
        public PerioxesController(PeripatoiDbContext dbContext, IPerioxhRepository perioxhRepository)
        {
            this.dbContext = dbContext;
            this.perioxhRepository = perioxhRepository;
        }

        //https://localhost:7229/api/perioxes
        //Ληψη ολων των περιοχων
        [HttpGet]
        //[Authorize(Roles = "read")]
        public async Task<IActionResult> GetAll()
        {
            var perioxesDomain = await perioxhRepository.GetAllAsync();

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
        //[Authorize(Roles = "read")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var perioxhDomain = await perioxhRepository.GetByIdAsync(id);

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
        //[Authorize(Roles = "write")]
        public async Task<IActionResult> Create([FromBody] AddPerioxhRequestDto addPerioxhRequestDto)
        {
            // εδω ελεγχουμε τα validations και εαν παραβιαζονται θα επιστρεφει 400 με το μηνυμα σφαλματος που αρμοζει
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var perioxhDomainModel = new Perioxh
            {
                Kwdikos = addPerioxhRequestDto.Kwdikos,
                Onoma = addPerioxhRequestDto.Onoma,
                EikonaUrl = addPerioxhRequestDto.EikonaUrl
            };

            await perioxhRepository.CreateAsync(perioxhDomainModel);

            var perioxhDto = new PerioxhDto
            {
                Id = perioxhDomainModel.Id,
                Onoma = perioxhDomainModel.Onoma,
                Kwdikos = perioxhDomainModel.Kwdikos,
                EikonaUrl = perioxhDomainModel.EikonaUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = perioxhDto.Id }, perioxhDto);
        }

        //https://localhost:7229/api/perioxes/{id}
        //Επεξεργασια περιοχης
        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "write")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePerioxhRequestDto updatePerioxhRequestDto)
        {
            // εδω ελεγχουμε τα validations και εαν παραβιαζονται θα επιστρεφει 400 με το μηνυμα σφαλματος που αρμοζει
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //πρωτα πρεπει να γινει map το dto σε μοντελο παλι
            var perioxhModel = new Perioxh
            {
                Kwdikos = updatePerioxhRequestDto.Kwdikos,
                Onoma = updatePerioxhRequestDto.Onoma,
                EikonaUrl = updatePerioxhRequestDto.EikonaUrl
            };

            perioxhModel = await perioxhRepository.UpdateAsync(id, perioxhModel);


            if (perioxhModel == null)
            {
                return NotFound(); // εαν το repository επιστρεψει null τοτε και εμεις επιστρεφουμε στον χρηστη not found
            }

            // τελος τα περναμε ολα στο dto και το στελνουμε πισω στον χρηστη με 200αρι
            var perioxhDto = new PerioxhDto
            {
                Id = perioxhModel.Id,
                Kwdikos = perioxhModel.Kwdikos,
                Onoma = perioxhModel.Onoma,
                EikonaUrl = perioxhModel.EikonaUrl
            };

            return Ok (perioxhDto);
        }

        //https://localhost:7229/api/perioxes/{id}
        //Διαγραφη περιοχης
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "write")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // με τον ιδιο ακριβως τροπου που υλοποιησαμε το update εγγραφης θα υλοποιηοσουμε και την διαγραφη
            // αρχικα ψαχνουμε μεσα απο τη μεθοδο του repository εαν υπαρχει περιοχη με το id που μας περασε ο χρηστης
            var perioxh = await perioxhRepository.DeleteAsync(id);

            if (perioxh == null)
            {
                return NotFound(); // εαν το dbcontext επιστρεψει στην περιοχη null τοτε και εμεις επιστρεφουμε στον χρηστη not found
            }

            var perioxhDto = new PerioxhDto
            {
                Id = perioxh.Id,
                Kwdikos = perioxh.Kwdikos,
                Onoma = perioxh.Onoma,
                EikonaUrl = perioxh.EikonaUrl
            };

            return Ok(perioxhDto);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using peripatoiCrud.API.Data;

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
            var perioxes = dbContext.Perioxes.ToList();
            return Ok(perioxes);
        }
        //https://localhost:7229/api/perioxes/{id}
        //Ληψη συγκεκριμενης περιοχης βαση του id της
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var perioxh = dbContext.Perioxes.FirstOrDefault(x => x.Id == id);

            if (perioxh == null) //ελεγχος εαν υπαρχει στη βαση, στην περιπτωση που δεν υπαρχει επιστρεφουμε 404
            {
                return NotFound();
            }

            return Ok(perioxh);
        }
    }
}

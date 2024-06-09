using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using peripatoiCrud.API.Models.Domain;
using peripatoiCrud.API.Models.DTOs;
using peripatoiCrud.API.Repositories;

namespace peripatoiCrud.API.Controllers
{
    //
    [Route("api/[controller]")]
    [ApiController]
    public class PeripatoiController : ControllerBase
    {
        private readonly IPeripatosRepository peripatosRepository;

        public PeripatoiController(IPeripatosRepository peripatosRepository)
        {
            this.peripatosRepository = peripatosRepository;
        }

        //https://localhost:7229/api/peripatoi
        //Ληψη ολων των περιοχων
        [HttpGet]
        //[Authorize(Roles = "read")]
        public async Task<IActionResult> GetAll([FromQuery] string? filter, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool afksousa, [FromQuery] int arithmosSelidwn = 1, [FromQuery] int megethosSelidas = 1000)
        {
            var peripatoiDomain = await peripatosRepository.GetAllAsync(filter, filterQuery, sortBy, afksousa, arithmosSelidwn, megethosSelidas);

            var peripatoiDto = new List<PeripatosDto>();

            foreach (var peripatosDomain in peripatoiDomain)
            {
                peripatoiDto.Add(new PeripatosDto()
                {
                    Id = peripatosDomain.Id,
                    Onoma = peripatosDomain.Onoma,
                    Perigrafh = peripatosDomain.Perigrafh,
                    Mhkos = peripatosDomain.Mhkos,
                    EikonaUrl = peripatosDomain.EikonaUrl,
                    DyskoliaId = peripatosDomain.DyskoliaId,
                    PerioxhId = peripatosDomain.PerioxhId,
                    Perioxh = peripatosDomain.Perioxh != null ? MapPerioxhToDto(peripatosDomain.Perioxh) : null,
                    Dyskolia = peripatosDomain.Dyskolia != null ? MapDyskoliaToDto(peripatosDomain.Dyskolia) : null
                });
            }

            return Ok(peripatoiDto);
        }


        //https://localhost:7229/api/peripatoi
        //δημιουργια περιπατου
        [HttpPost]
        //[Authorize(Roles = "write")]
        public async Task<IActionResult> Create([FromBody] AddPeripatosRequestDto addPeripatosRequestDto)
        {
            // εδω ελεγχουμε τα validations και εαν παραβιαζονται θα επιστρεφει 400 με το μηνυμα σφαλματος που αρμοζει
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var peripatos = new Peripatos()
            {
                Onoma = addPeripatosRequestDto.Onoma,
                Perigrafh = addPeripatosRequestDto.Perigrafh,
                Mhkos = addPeripatosRequestDto.Mhkos,
                EikonaUrl = addPeripatosRequestDto.EikonaUrl,
                DyskoliaId = addPeripatosRequestDto.DyskoliaId,
                PerioxhId = addPeripatosRequestDto.PerioxhId
            };

            var peripatosResult = await peripatosRepository.CreateAsync(peripatos);

            // κανουμε map σε dto για επιστροφη στον client με 200αρι
            var peripatosDto = new PeripatosDto()
            {
                Onoma = peripatosResult.Onoma,
                Perigrafh = peripatosResult.Perigrafh,
                Mhkos = peripatosResult.Mhkos,
                EikonaUrl = peripatosResult.EikonaUrl,
                DyskoliaId = peripatosResult.DyskoliaId,
                PerioxhId = peripatosResult.PerioxhId
            };

            return Ok(peripatosDto);
        }

        //https://localhost:7229/api/peripatoi/{id}
        //Ληψη συγκεκριμένου περιπατου βαση του id του
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "read")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var peripatosDomain = await peripatosRepository.GetByIdAsync(id);

            if (peripatosDomain == null) //ελεγχος εαν υπαρχει στη βαση, στην περιπτωση που δεν υπαρχει επιστρεφουμε 404
            {
                return NotFound();
            }

            //κανουμε εδω το μαπ και μετα επιστρεφουμε τον περιπατο
            var peripatosDto = new PeripatosDto()
            {
                Id = peripatosDomain.Id,
                Onoma = peripatosDomain.Onoma,
                Perigrafh = peripatosDomain.Perigrafh,
                Mhkos = peripatosDomain.Mhkos,
                EikonaUrl = peripatosDomain.EikonaUrl,
                DyskoliaId = peripatosDomain.DyskoliaId,
                PerioxhId = peripatosDomain.PerioxhId,
                Perioxh = peripatosDomain.Perioxh != null ? MapPerioxhToDto(peripatosDomain.Perioxh) : null,
                Dyskolia = peripatosDomain.Dyskolia != null ? MapDyskoliaToDto(peripatosDomain.Dyskolia) : null
            };

            return Ok(peripatosDto);
        }


        //https://localhost:7229/api/peripatoi/{id}
        //Επεξεργασια περιπατου
        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "write")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePeripatosRequestDto updatePeripatosRequestDto)
        {
            // εδω ελεγχουμε τα validations και εαν παραβιαζονται θα επιστρεφει 400 με το μηνυμα σφαλματος που αρμοζει
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var peripatosModel = new Peripatos
            {
                Id = id, // Ensure the Id is set correctly
                Onoma = updatePeripatosRequestDto.Onoma,
                Perigrafh = updatePeripatosRequestDto.Perigrafh,
                Mhkos = updatePeripatosRequestDto.Mhkos,
                EikonaUrl = updatePeripatosRequestDto.EikonaUrl,
                DyskoliaId = updatePeripatosRequestDto.DyskoliaId,
                PerioxhId = updatePeripatosRequestDto.PerioxhId
            };

            peripatosModel = await peripatosRepository.UpdateAsync(id, peripatosModel);

            if (peripatosModel == null)
            {
                return NotFound(); // 404
            }

            // 200
            var peripatosDto = new PeripatosDto
            {
                Id = peripatosModel.Id, // Ensure the Id is set correctly
                Onoma = peripatosModel.Onoma,
                Perigrafh = peripatosModel.Perigrafh,
                Mhkos = peripatosModel.Mhkos,
                EikonaUrl = peripatosModel.EikonaUrl,
                DyskoliaId = peripatosModel.DyskoliaId,
                PerioxhId = peripatosModel.PerioxhId
            };

            return Ok(peripatosDto);
        }


        //https://localhost:7229/api/peripatoi/{id}
        //Διαγραφη περιπατου
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "write")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var peripatos = await peripatosRepository.DeleteAsync(id);

            if (peripatos == null)
            {
                return NotFound(); //404
            }

            //200
            var peripatosDto = new PeripatosDto
            {
                Id = peripatos.Id,
                Onoma = peripatos.Onoma,
                Perigrafh = peripatos.Perigrafh,
                Mhkos = peripatos.Mhkos,
                EikonaUrl = peripatos.EikonaUrl,
                DyskoliaId = peripatos.DyskoliaId,
                PerioxhId = peripatos.PerioxhId
            };

            return Ok(peripatosDto);
        }

        //εαν υλοποιηθει σε επομενο sprint το work item #0019 θα διαγραφουν αυτες οι μεθοδοι για τα mappings
        private PerioxhDto MapPerioxhToDto(Perioxh perioxh)
        {
            return new PerioxhDto
            {
                Id = perioxh.Id,
                Onoma = perioxh.Onoma,
                Kwdikos = perioxh.Kwdikos,
                EikonaUrl = perioxh.EikonaUrl

            };
        }

        private DyskoliaDto MapDyskoliaToDto(Dyskolia dyskolia)
        {
            return new DyskoliaDto
            {
                Id = dyskolia.Id,
                Onoma = dyskolia.Onoma
            };
        }
    }
}

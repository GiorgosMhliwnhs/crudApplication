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
        //δημιουργια περιπατου
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddPeripatosRequestDto addPeripatosRequestDto)
        {
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
    }
}

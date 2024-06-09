using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Peripatoi.UI.Models;
using Peripatoi.UI.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace Peripatoi.UI.Controllers
{
    public class PeripatoiController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public PeripatoiController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<PeripatosDto> response = new List<PeripatosDto>();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponse = await client.GetAsync("https://localhost:7229/api/peripatoi");
                httpResponse.EnsureSuccessStatusCode();
                response.AddRange(await httpResponse.Content.ReadFromJsonAsync<IEnumerable<PeripatosDto>>());
            }
            catch (Exception ex)
            {
                // να δημιουργηθει view για error με το exception
            }

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Prosthiki()
        {
            var client = httpClientFactory.CreateClient();

            var perioxesResponse = await client.GetFromJsonAsync<List<PerioxhDto>>("https://localhost:7229/api/perioxes");

            var dyskolies = new List<SelectListItem> //εδω κανονικα θα καναμε call σε καποιο api endpoint για τις δυσκολιες αλλα δεν υπαρχει, οποτε για την ωρα κανουμε hardcore τις δυσκολιες με τα αναλογα GUIDs
            {
                new SelectListItem { Value = "09061773-9946-4C79-804E-0F33F6C23213", Text = "ΕΥΚΟΛΟΣ" },
                new SelectListItem { Value = "F4DB66CF-1936-48FF-B8E7-8C99701BCFD9", Text = "ΜΕΤΡΙΟΣ" },
                new SelectListItem { Value = "CA2F6118-2F8F-4CA5-99F5-31287E3DCF15", Text = "ΔΥΣΚΟΛΟΣ" }
            };

            var viewModel = new ProsthikiPeripatouViewModel
            {
                Perioxes = perioxesResponse?.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Onoma }).ToList() ?? new List<SelectListItem>(),
                Dyskolies = dyskolies
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Prosthiki(ProsthikiPeripatouViewModel viewModel)
        {
            var client = httpClientFactory.CreateClient();
            var jsonPayload = JsonSerializer.Serialize(viewModel);
            //System.Diagnostics.Debug.WriteLine($"Payload: {jsonPayload}");

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7229/api/peripatoi"),
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            var httpResponse = await client.SendAsync(httpRequestMessage);

            if (!httpResponse.IsSuccessStatusCode)
            {
                // εδω θα κανουμε handle το error
                return StatusCode((int)httpResponse.StatusCode, await httpResponse.Content.ReadAsStringAsync());
            }

            var response = await httpResponse.Content.ReadFromJsonAsync<PeripatosDto>();

            if (response != null)
            {
                return RedirectToAction("Index", "Peripatoi");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Epeksergasia(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var peripatosResponse = await client.GetFromJsonAsync<PeripatosDto>($"https://localhost:7229/api/peripatoi/{id.ToString()}");
            var perioxesResponse = await client.GetFromJsonAsync<List<PerioxhDto>>("https://localhost:7229/api/perioxes");

            var difficulties = new List<SelectListItem> //εδω κανονικα θα καναμε call σε καποιο api endpoint για τις δυσκολιες αλλα δεν υπαρχει, οποτε για την ωρα κανουμε hardcore τις δυσκολιες με τα αναλογα GUIDs
            {
                new SelectListItem { Value = "09061773-9946-4C79-804E-0F33F6C23213", Text = "ΕΥΚΟΛΟΣ" },
                new SelectListItem { Value = "CA2F6118-2F8F-4CA5-99F5-31287E3DCF15", Text = "ΜΕΤΡΙΟΣ" },
                new SelectListItem { Value = "F4DB66CF-1936-48FF-B8E7-8C99701BCFD9", Text = "ΔΥΣΚΟΛΟΣ" }
            };

            var viewModel = new ProsthikiPeripatouViewModel
            {
                Perioxes = perioxesResponse?.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Onoma }).ToList() ?? new List<SelectListItem>(),
                Dyskolies = difficulties
            };

            if (peripatosResponse != null)
            {
                var perioxhResponse = await client.GetFromJsonAsync<PerioxhDto>($"https://localhost:7229/api/perioxes/{peripatosResponse.PerioxhId.ToString()}");
                viewModel.Id = peripatosResponse.Id;
                viewModel.Onoma = peripatosResponse.Onoma;
                viewModel.Perigrafh = peripatosResponse.Perigrafh;
                viewModel.Mhkos = peripatosResponse.Mhkos;
                viewModel.EikonaUrl = peripatosResponse.EikonaUrl;
                viewModel.DyskoliaId = peripatosResponse.DyskoliaId;
                viewModel.PerioxhId = perioxhResponse?.Id ?? Guid.Empty;
            }

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Epeksergasia(PeripatosDto request)
        {
            var client = httpClientFactory.CreateClient();
            var jsonPayload = JsonSerializer.Serialize(request);
            //System.Diagnostics.Debug.WriteLine($"Payload: {jsonPayload}");

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7229/api/peripatoi/{request.Id}"),
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                // εδω θα κανουμε handle το error
                return StatusCode((int)httpResponseMessage.StatusCode, await httpResponseMessage.Content.ReadAsStringAsync());
            }

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<PeripatosDto>();

            if (response != null)
            {
                return RedirectToAction("Index", "Peripatoi");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Diagrafh(PeripatosDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7229/api/peripatoi/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Peripatoi");
            }
            catch (Exception ex)
            {
                //μηνυμα σφαλματος
            }

            return View();
        }
    }
}

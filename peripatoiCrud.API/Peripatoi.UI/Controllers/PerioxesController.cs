using Microsoft.AspNetCore.Mvc;
using Peripatoi.UI.Models.DTOs;

namespace Peripatoi.UI.Controllers
{
    public class PerioxesController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public PerioxesController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> Index()
        {
            List<PerioxhDto> response = new List<PerioxhDto>();
            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponse = await client.GetAsync("https://localhost:7229/api/perioxes");
                httpResponse.EnsureSuccessStatusCode();
                response.AddRange(await httpResponse.Content.ReadFromJsonAsync<IEnumerable<PerioxhDto>>());
            }
            catch (Exception ex)
            {
                // να δημιουργηθει view για error με το exception
            }

            return View(response);
        }

    }
}

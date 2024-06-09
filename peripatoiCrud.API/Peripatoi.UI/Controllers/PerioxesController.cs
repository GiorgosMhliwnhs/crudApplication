﻿using Microsoft.AspNetCore.Mvc;
using Peripatoi.UI.Models;
using Peripatoi.UI.Models.DTOs;
using System.Text;
using System.Text.Json;

namespace Peripatoi.UI.Controllers
{
    public class PerioxesController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public PerioxesController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> Prosthiki()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Prosthiki(ProsthikiPerioxhsViewModel viewModel)
        {
            var client = httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7229/api/perioxes"),
                Content = new StringContent(JsonSerializer.Serialize(viewModel), Encoding.UTF8, "application/json")
            };

            var httpResponse = await client.SendAsync(httpRequestMessage);

            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadFromJsonAsync<PerioxhDto>();

            if (response != null)
            {
                return RedirectToAction("Index", "Perioxes");
            }

            return View();
        }
    }
}
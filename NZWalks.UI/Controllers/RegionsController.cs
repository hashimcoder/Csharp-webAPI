using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();

            try
            {
                var client = httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("http://localhost:5080/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content
                    .ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception)
            {
                ViewBag.Error = "Unable to load regions. Please ensure the API and database are running.";
            }

            return View(response);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

      
        // POST: Regions/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegionViewModel request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://localhost:5080/api/regions"),
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
                    if (response != null)
                    {
                        return RedirectToAction("Index", "Regions");
                    }
                }
                else
                {
                    var errorResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Failed to create region: {errorResponse}");
                    return View(request);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to create region. Please ensure the API is running.");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
         var client = httpClientFactory.CreateClient();
         var response = await client.GetFromJsonAsync<RegionDto>($"http://localhost:5080/api/regions/{id}");
         if (response is not null)
         {
            return View(response);
         }
         return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, RegionDto request)
        {
             var client = httpClientFactory.CreateClient();
             var httpRequestMessage = new HttpRequestMessage()
             {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"http://localhost:5080/api/regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
             };
             var httpResponseMessage = await client.SendAsync(httpRequestMessage);
             
             if (httpResponseMessage.IsSuccessStatusCode)
             {
                 var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
                 if (response is not null)
                 {
                     return RedirectToAction("Index", "Regions");
                 }
             }
             else
             {
                 var errorResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                 ModelState.AddModelError(string.Empty, errorResponse);
             }

             return View(request);
        }
        
    }

}
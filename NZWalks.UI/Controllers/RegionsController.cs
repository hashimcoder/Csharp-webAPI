using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        public RegionsController(IHttpClientFactory  httpClientFactory)
        {
          this.httpClientFactory = httpClientFactory;   
        }
        public  async Task<IActionResult> Index()
        {
             List<RegionDto> response = new List<RegionDto>();

            try
            {
                  // Get all regions from webapi
             var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("http://localhost:5081/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {
               ViewBag.Error = "Unable to load regions. Please ensure the API and database are running.";
            }
            
             
            return View(response);
        }
    }
}

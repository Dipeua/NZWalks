using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;

namespace NZWalks.UI.Controllers
{
    public class RegionsController(IHttpClientFactory _httpClientFactory) : Controller
    {
        // GET
        public async Task<IActionResult> Index()
        {
            List<RegionDto> responseBody = new List<RegionDto>();

            try
            {
                // Get api information
                var client = _httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync("https://localhost:7104/api/regions");
                
                // check if it's OK!
                httpResponseMessage.EnsureSuccessStatusCode();

                responseBody.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<List<RegionDto>>());

            }
            catch(Exception ex)
            {
                throw;
            }
            return View(responseBody);
        }
    }
}

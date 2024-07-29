using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _url;
        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _url = "https://localhost:7104/api/regions";
        }
        // GET
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> responseBody = new List<RegionDto>();

            try
            {
                // Get api information
                var client = _httpClientFactory.CreateClient();
                var httpResponseMessage = await client.GetAsync(_url);
                
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Create(AddRegion addRegion)
        {
            var client = _httpClientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_url),
                Content = new StringContent(JsonSerializer.Serialize(addRegion), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var responseBody = await httpRequestMessage.Content.ReadFromJsonAsync<AddRegion>();
            
            if(responseBody is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View(responseBody);
        }

    }
}

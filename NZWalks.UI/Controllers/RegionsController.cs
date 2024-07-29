using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using System.Net.Http;
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
            var responseBody = new List<RegionDto>();

            try
            {
                // Get api information
                var client = _httpClientFactory.CreateClient();
                //var httpResponseMessage = await client.GetAsync(_url);

                //// check if it's OK!
                //httpResponseMessage.EnsureSuccessStatusCode();

                //responseBody.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<List<RegionDto>>());

                var response = await client.GetFromJsonAsync<List<RegionDto>>($"{_url}");

                if (response is not null)
                {
                    return View(response);
                }

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


        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            // Get api information
            var client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetFromJsonAsync<RegionDto>($"{_url}/{Id.ToString()}");
                if (response == null)
                {
                    return NotFound();
                }
                return View(response);
            }
            catch (HttpRequestException ex)
            {
                // Log the exception or handle it as necessary
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the server.");
            }
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


        //[HttpPost]
        //public async Task<IActionResult> Edit(AddRegion addRegion)
        //{
        //    var client = _httpClientFactory.CreateClient();
        //    var httpRequestMessage = new HttpRequestMessage()
        //    {
        //        Method = HttpMethod.Put,
        //        RequestUri = new Uri(_url),
        //        Content = new StringContent(JsonSerializer.Serialize(addRegion), Encoding.UTF8, "application/json")
        //    };
        //    var httpResponseMessage = await client.SendAsync(httpRequestMessage);
        //    httpResponseMessage.EnsureSuccessStatusCode();

        //    var responseBody = await httpRequestMessage.Content.ReadFromJsonAsync<AddRegion>();

        //    if (responseBody is not null)
        //    {
        //        return RedirectToAction("Index", "Regions");
        //    }

        //    return View(responseBody);
        //}
    }
}

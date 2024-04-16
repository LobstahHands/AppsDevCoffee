using AppsDevCoffee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Diagnostics;
using Newtonsoft.Json;

namespace AppsDevCoffee.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient httpClient;
        public HomeController() 
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://coffee.alexflipnote.dev/");
        }
        public async Task<IActionResult> Index()
        {
            //var homePicture = await
            await GetHomePicture();
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        private async Task GetHomePicture()
        {
            try
            {
                var response = await httpClient.GetAsync("/random.json");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject<dynamic>(json);

                ViewBag.HomePicture = data.file;
            }
            catch (HttpRequestException ex)
            {
                // Log or handle the exception
                ViewBag.HomePicture = "/images/cup_white.jpg";
            }
        }

    }
}

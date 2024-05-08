using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> About()
        {
            await GetHomePicture();
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

                ViewBag.ApiPicture = data.file;
            }
            catch (HttpRequestException ex)
            {
                // Log or handle the exception
                ViewBag.ApiPicture = "/images/coffee_cement.jpg";
            }
        }

    }
}

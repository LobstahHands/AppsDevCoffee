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
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            //Generate a URL for a random coffee related picture - API Call
            await GetHomePicture();
            return View();
        }

        private async Task GetHomePicture()
        {
            //API Implementation. This returns the URL for a random picture that is coffee related.
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
                //If an exception occurs, use a default URL. 
                ViewBag.ApiPicture = "/images/coffee_cement.jpg";
            }
        }

    }
}

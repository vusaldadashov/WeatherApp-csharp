using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using WeatherApp.Models;
namespace WeatherApp.Controllers
{
    public class HomeController : Controller
    {

        public static List<TempViewModel> newTemps = new List<TempViewModel>();
        private static string ErrorMessage = "";
        public async Task<string> MakeRequest(string search_key)
        {
            using (HttpClient client = new HttpClient())
            {
                string main_url = "http://api.weatherapi.com/v1/current.json?" + "key=04bec25fbd46464197070259230905" + "&q=" + search_key;
                HttpResponseMessage response = await client.GetAsync(main_url);

                string rs = await response.Content.ReadAsStringAsync();
                return rs;
            }

        }
        public IActionResult Index()
        {
            ViewBag.ErrorMessage = ErrorMessage;
            return View(newTemps);
        }

        public async Task<IActionResult> GetTemp(string search_key)
        {
            if (string.IsNullOrWhiteSpace(search_key))
            {
                ErrorMessage = "Please write something to location!";
                return RedirectToAction("Index");
            }
            if (newTemps.Find(item => item.name == search_key) != null){
                ErrorMessage = "This location is exist already!";
                return RedirectToAction("Index");
            }
            
            string response = "";
            try
            {
                response = await MakeRequest(search_key);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Something happened bad!";
                return RedirectToAction("Index");
            }

            JObject? jObject = JsonConvert.DeserializeObject<JObject>(response);
            string name = jObject["location"]["name"].ToString();
            string temp_c = jObject["current"]["temp_c"].ToString();
            string temp_f = jObject["current"]["temp_f"].ToString();
            string text = jObject["current"]["condition"]["text"].ToString();
            string imageUrl = jObject["current"]["condition"]["icon"].ToString();
            Debug.Write(jObject);
         
            TempViewModel newTemp = new TempViewModel()
            {
                name = name,
                temp_c = temp_c,
                temp_f = temp_f,
                text = text,
                imageUrl = imageUrl,
            };


            ErrorMessage = "";
            newTemps.Add(newTemp);
            return RedirectToAction("Index");

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
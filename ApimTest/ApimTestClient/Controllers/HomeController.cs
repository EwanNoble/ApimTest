using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApimTest.Models;
using ApimTestClient.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace ApimTest.Controllers
{
    public class HomeController : Controller
    {

        IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new IndexViewModel(_configuration["ApiUrl"]));
        }

        [HttpPost]
        public async Task<IActionResult> Index(string input)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage res = await client.GetAsync(input))
                using (HttpContent content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    return View(new IndexViewModel(input) { Text = $"{res.StatusCode} - {data}" });
                }
            }
            catch (Exception e)
            {
                return View(new IndexViewModel(input) { Text = $"{e.Message}\n{e.InnerException}" });
            }
        }
    }
}

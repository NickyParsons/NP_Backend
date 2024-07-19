using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestAspNetApplication.Data;
using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Services;

namespace TestAspNetApplication.Controllers
{
    public class HomeController : Controller
    {
        ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger) 
        {
            _logger = logger;
        }
        [HttpGet]
        [Route("/test3")]
        [Authorize]
        public IActionResult GetTestJson()
        {
            var returnJson = new { name = "ass" };
            return Json( returnJson );
        }
    }
}

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
        [Route("/test2")]
        public IActionResult GetTestJson()
        {
            _logger.LogInformation("New requyest in test 2");
            var returnJson = new { name = "test 2" };
            return Json(returnJson);
        }
        [HttpGet]
        [Route("/test3")]
        [Authorize]
        public IActionResult GetTestAuthorizeJson()
        {
            _logger.LogInformation("New requyest in test 3");
            var returnJson = new { name = "test 3" };
            return Json( returnJson );
        }
    }
}

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
        [Route("/")]
        [HttpGet]
        public async Task ShowHomePage()
        {
            Request.Headers["Cache-Control"] = "no-cache";
            Response.Headers["Cache-Control"] = "no-cache";
            Response.ContentType = "text/html; charset=utf-8";
            await Response.SendFileAsync("wwwroot/html/indexPage.html");
        }
        [Authorize(Roles = "Admin")]
        [Route("/counter")]
        [HttpGet]
        public async Task ShowCounterPage()
        {
            Response.ContentType = "text/html; charset=utf-8";
            await Response.SendFileAsync("wwwroot/html/counterPage.html");
        }
    }
}

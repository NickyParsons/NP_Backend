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
        public async Task ShowLayout()
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.StatusCode = 200;
            await Response.SendFileAsync("wwwroot/html/_layout.html");
        }
        [Route("/home")]
        [HttpGet]
        public async Task ShowHomePage()
        {
            Response.ContentType = "text/html; charset=utf-8";
            Response.StatusCode = 200;
            await Response.SendFileAsync("wwwroot/html/components/home.html");
        }
        [Authorize(Roles = "Admin")]
        [Route("/counter")]
        [HttpGet]
        public async Task ShowCounterPage()
        {
            Response.ContentType = "text/html; charset=utf-8";
            await Response.SendFileAsync("wwwroot/html/Components/Counter.html");
        }
        [HttpGet]
        [Route("/test1")]
        public async Task GetTestHtml1()
        {
            _logger.LogDebug($"Test 1 requested");
            Response.ContentType = "text/javascript; charset=utf-8";
            Response.StatusCode = 200;
            await Response.SendFileAsync("wwwroot/html/Components/Test1.html");
        }
        [HttpGet]
        [Route("/test2")]
        public async Task GetTestHtml2()
        {
            _logger.LogDebug($"Test 2 requested");
            Response.ContentType = "text/html; charset=utf-8";
            await Response.SendFileAsync("wwwroot/html/Components/Test2.html");
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using TestAspNetApplication.Data;
using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Services;

namespace TestAspNetApplication.Controllers
{
    public class HomeController : Controller
    {
        ILogger<HomeController> _logger;
        IRoleRepository _roleRepository;
        public HomeController(ILogger<HomeController> logger, IRoleRepository roleRepository) 
        {
            _logger = logger;
            _roleRepository = roleRepository;
        }
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> GetRoles()
        {
            _logger.LogInformation("New request in test. Trying to get all roles");
            var roles = await _roleRepository.GetAllRoles();
            return Json(roles);
        }
        [HttpGet]
        [Route("/test2")]
        public IActionResult GetTestJson()
        {
            _logger.LogInformation("New request in test 2");
            var returnJson = new { name = "test 2" };
            return Json(returnJson);
        }
        [HttpGet]
        [Route("/test3")]
        [Authorize]
        public IActionResult GetTestAuthorizeJson()
        {
            _logger.LogInformation("New request in test 3");
            var returnJson = new { name = "test 3" };
            return Json( returnJson );
        }
    }
}

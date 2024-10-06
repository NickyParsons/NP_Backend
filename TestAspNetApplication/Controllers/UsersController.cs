using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Services;
using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Controllers
{
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger { get; set; }
        private UserService _userService;
        private UserRepository _userRepository;
        public UsersController(ILogger<UsersController> logger, UserService userService, UserRepository userRepository)
        {
            _logger = logger;
            _userService = userService;
            _userRepository = userRepository;
        }
        [Route("/users/{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> GetUserData(Guid id)
        {
            var user = await _userRepository.GetUserById(id, false);
            if (user != null)
            {
                return Json(user!);
            }
            else
            {
                return NotFound();
            }
        }
        [Authorize]
        [Route("/users/{id:guid}/edit")]
        [HttpPost]
        public async Task<IActionResult> EditCurrentUser(EditUserRequest form)
        {
            _logger.LogInformation($"Edit user method");
            var userId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (userId != form.Id)
            {
                return Unauthorized();
            }
            User user;
            if (Request.Form.Files.Count != 0)
            {
                user = await _userService.EditUser(form, Request.Form.Files.First());
            }
            else
            {
                user = await _userService.EditUser(form, null);
            }
            user.Articles.Clear();
            user.Role = null;
            return Json(user);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Services;

namespace TestAspNetApplication.Controllers
{
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger { get; set; }
        private IUserService _userService;
        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> RegisterNewUser(RegisterUserRequest form)
        {
            _logger.LogDebug($"Регистрация нового пользователя {form.Email} {form.Firstname} {form.Lastname}");
            await _userService.Register(form);
            return Ok();
        }
        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginUserRequest form)
        {
            string? token = await _userService.Login(form);
            if (token != null)
            {
                _logger.LogDebug($"Successfull login: /'{form.Email}/'");
                //Response.Cookies.Append("nasty-boy", token);
                var returnJson = new { token = token };
                return Json( returnJson );
            }
            else
            {
                _logger.LogWarning($"Email or password incorrect");
                return Unauthorized();
            }
        }
        [Route("/logout")]
        [HttpGet]
        public IActionResult LogoutUser()
        {
            Response.Cookies.Delete("nasty-boy");
            return Redirect("/");
        }
    }
}

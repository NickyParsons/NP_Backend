using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Services;

namespace TestAspNetApplication.Controllers
{
    public class UsersController : Controller
    {
        private ILogger<UsersController> _logger { get; set; }
        private AuthService _authService;
        private IUserRepository _userRepository;
        public UsersController(ILogger<UsersController> logger, AuthService authService, IUserRepository userRepository)
        {
            _logger = logger;
            _authService = authService;
            _userRepository = userRepository;
        }
        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> RegisterNewUser(RegisterUserRequest form)
        {
            _logger.LogDebug($"Регистрация нового пользователя {form.Email} {form.Firstname} {form.Lastname}");
            await _authService.Register(form);
            return Ok();
        }
        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginUserRequest form)
        {
            string? token = await _authService.Login(form);
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
        [Route("/users/{id:guid}")]
        [HttpGet]
        public async Task<IActionResult> GetUserData(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user != null)
            {
                return Json(user!);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

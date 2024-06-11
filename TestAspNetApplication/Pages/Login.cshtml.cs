using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using TestAspNetApplication.Services;

namespace TestAspNetApplication.Pages
{
    [IgnoreAntiforgeryToken]
    public class LoginModel : PageModel
    {
        public string? Message { get; set; }
        private readonly ILogger<LoginModel> _logger;
        private readonly IUserService _userService;
        public LoginModel(ILogger<LoginModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        public async Task<IActionResult> OnPost(string email, string password, string? returnUrl)
        {
            //_logger.LogDebug($"RETURN URL: {returnUrl ?? "NULL"}");
            var token = await _userService.Login(email, password);
            PageContext.HttpContext.Response.Cookies.Append("nasty-boy", token);
            return RedirectToPage("Index");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using TestAspNetApplication.Services;
using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Pages
{
    [IgnoreAntiforgeryToken]
    public class RegisterModel : PageModel
    {
        public string? Message {  get; set; }
        private readonly ILogger<RegisterModel> _logger;
        private readonly IUserService _userService;

        public RegisterModel(ILogger<RegisterModel> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        public async Task<IActionResult> OnPost(string email, string password, string passwordRepeat, string? firstname, string? lastname)
        {
            if (password != passwordRepeat)
            {
                Message = "Пароли не совпадают";
            }
            else
            {
                await _userService.Register(email, password, firstname, lastname);
            }
            return Page();
        }
    }
}

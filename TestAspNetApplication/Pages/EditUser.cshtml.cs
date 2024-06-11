using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;

namespace TestAspNetApplication.Pages
{
    [IgnoreAntiforgeryToken]
    public class CreateUserModel : PageModel
    {
        IPersonRepository _repo;
        ILogger<CreateUserModel> _logger;
        public Person? RequestedUser { get; set; }
        public CreateUserModel(IPersonRepository repo, ILogger<CreateUserModel> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public async Task<IActionResult> OnPost(int id)
        {
            RequestedUser = await _repo.GetByIdAsync(id);
            return Page();
        }
    }
}

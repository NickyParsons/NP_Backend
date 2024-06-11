using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestAspNetApplication.Data;

namespace TestAspNetApplication.Pages
{
    public class IndexModel : PageModel
    {
        public string Message { get; set; } = "";
        public IndexModel() 
        {
            
        }
        public void OnGet([FromServices] ILogger<IndexModel> logger, [FromServices] IConfiguration configuration, [FromServices] IUserRepository userRepo)
        {
            Message = "Welcum";
        }
    }
}

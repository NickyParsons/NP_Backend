using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

using TestAspNetApplication.FileLogger;
using TestAspNetApplication.Data;
using TestAspNetApplication.Services;
using TestAspNetApplication.Data.Entities;


namespace TestAspNetApplication.Pages
{
    [IgnoreAntiforgeryToken]
    [Authorize(Roles = "admin")]
    public class PersonsModel : PageModel
    {
        IPersonRepository _repo;
        ILogger<PersonsModel> _logger;
        public bool HasRequestId { get; set; }
        public Person? RequestedUser { get; set; }
        public IEnumerable<Person> People { get; set; }
        
        public PersonsModel(IPersonRepository repo, ILogger<PersonsModel> logger)
        {
            _repo = repo;
            _logger = logger;
            People = new List<Person>();
            HasRequestId = false;
        }
        public async Task<IActionResult> OnGetShow(int? id)
        {
            if (id == null)
            {
                HasRequestId = false;
                People = await _repo.GetAllAsync();
            }
            else
            {
                HasRequestId = true;
                RequestedUser = await _repo.GetByIdAsync(id.Value);
                if (RequestedUser == null)
                {
                    return NotFound();
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostCreate(string firstname, string lastname)
        {
            Person person = new Person { FirstName = firstname, LastName = lastname };
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files.First();
                person.PhotoPath = file.FileName;
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string filePath = Path.Combine(uploadPath, file.FileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            await _repo.CreateAsync(person);
            return RedirectToPage("Persons", "show");
        }
        public async Task<IActionResult> OnPostDelete(int? id)
        {
            if (id != null)
            {
                await _repo.DeleteAsync(id.Value);
            }
            return RedirectToPage("Persons", "show");
        }
        public async Task<IActionResult> OnPostEdit(Person person)
        {
            _logger.LogDebug($"POST EDIT {person.Id} {person.FirstName} {person.LastName}");
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files.First();
                person.PhotoPath = file.FileName;
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string filePath = Path.Combine(uploadPath, file.FileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            await _repo.UpdateAsync(person);
            return RedirectToPage("Persons", "show");
        }
    }
}

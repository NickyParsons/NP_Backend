using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestAspNetApplication.Data;
using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Services;

namespace TestAspNetApplication.Controllers
{
    public class ArticleController : Controller
    {
        private ILogger<ArticleController> _logger { get; set; }
        private ArticleService _articleService;
        public ArticleController(ILogger<ArticleController> logger, ArticleService articleService)
        {
            _logger = logger;
            _articleService = articleService;
        }
        [Authorize]
        [HttpPost]
        [Route("/articles/new")]
        public async Task<IActionResult> CreateNewArticle(CreateArticleRequest form)
        {
            _logger.LogDebug($"New request at CreateNewArticle()");
            form.Id = Guid.NewGuid();
            var imageFile = Request.Form.Files.First();
            string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\content\\articles\\{form.Id}");
            //string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\content\\articles\\{Guid.NewGuid()}");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            string uploadPath = Path.Combine(uploadDirectory, imageFile.FileName);
            using (FileStream fileStream = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            form.ImageUrl = uploadPath;
            //_logger.LogDebug($"Image Url in Controller: {form.ImageUrl}");
            await _articleService.CreateArticle(form);
            return Ok();
        }
    }
}

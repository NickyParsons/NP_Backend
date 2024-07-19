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
        private ArticleRepository _articleRepo;
        public ArticleController(ILogger<ArticleController> logger, ArticleRepository articleRepository)
        {
            _logger = logger;
            _articleRepo = articleRepository;
        }
        [HttpPost]
        [Route("/articles/new")]
        public async Task<IActionResult> CreateNewArticle(CreateArticleRequest form)
        {
            try
            {
                _logger.LogDebug($"New request at CreateNewArticle()");
                Article article = new Article();
                article.Id = Guid.NewGuid();
                article.Name = form.Name;
                article.Description = form.Description;
                article.Text = form.Text;
                var imageFile = Request.Form.Files.First();
                string uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\content\\articles\\{article.Id}");
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }
                string uploadPath = Path.Combine(uploadDirectory, imageFile.FileName);
                using (FileStream fileStream = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                article.Image = uploadPath;
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}

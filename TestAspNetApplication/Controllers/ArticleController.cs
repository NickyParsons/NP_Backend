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
            var userId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (form.AuthorId != userId) 
            {
                return BadRequest("Something wrong with AuthorID");
            }
            if (Request.Form.Files.Count == 0)
            {
                await _articleService.CreateArticle(form);
            }
            else
            {
                var imageFile = Request.Form.Files.First();
                await _articleService.CreateArticle(form, imageFile);
            }
            return Ok();
        }
        [HttpGet]
        [Route("/articles")]
        public async Task<IActionResult> GetArticles()
        {
            return Json(await _articleService.GetAllArticles());
        }
    }
}

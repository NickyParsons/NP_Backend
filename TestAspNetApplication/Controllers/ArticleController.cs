using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
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
        public ArticleController(
            ILogger<ArticleController> logger, 
            ArticleService articleService)
        {
            _logger = logger;
            _articleService = articleService;
        }
        [Authorize]
        [HttpPost]
        [Route("/articles")]
        public async Task<IActionResult> CreateNewArticle(CreateArticleRequest form)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (form.AuthorId != userId) 
            {
                _logger.LogDebug("User ID from cookie and user ID from request doesn't match");
                return BadRequest("Something wrong with AuthorID");
            }
            try
            {
                var files = Request.Form.Files;
                var article = await _articleService.CreateArticle(form, files.Count != 0 ? files.First() : null);
                _logger.LogInformation($"Article: \'{article.Id}\' created");
                return Json(article);
            }
            catch (BadHttpRequestException e)
            {
                _logger.LogDebug(e.Message);
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Route("/articles")]
        public async Task<IActionResult> GetArticles()
        {
            return Json(await _articleService.GetAllArticles());
        }
        [HttpGet]
        [Route("/articles/{articleId:guid}")]
        public async Task<IActionResult> GetArticleById(Guid articleId)
        {
            try
            {
                return Json(await _articleService.GetArticleById(articleId));
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("/articles/{articleId:guid}/edit")]
        public async Task<IActionResult> EditArticle(EditArticleRequest form)
        {
            var cookieId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (form.AuthorId != cookieId)
            {
                _logger.LogDebug("User ID from cookie and user ID from request doesn't match");
                return BadRequest("Something wrong with AuthorID");
            }
            var cookieRole = HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType);
            _logger.LogDebug($"Cookie role: {cookieRole}");
            bool moderRules = (cookieRole != null && (cookieRole.Value == "Admin" || cookieRole.Value == "Moder"));
            try
            {
                var files = Request.Form.Files;
                var article = await _articleService.EditArticle(form, moderRules, files.Count != 0 ? files.First() : null);
                _logger.LogInformation($"Article: \'{article.Id}\' edited");
                return Json(article);
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("/articles/{articleId:guid}/delete")]
        public async Task<IActionResult> DeleteArticle(DeleteArticleRequest form)
        {
            var cookieId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (form.AuthorId != cookieId)
            {
                _logger.LogDebug("User ID from cookie and user ID from request doesn't match");
                return BadRequest("Something wrong with AuthorID");
            }
            var cookieRole = HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType);
            _logger.LogDebug($"Cookie role: {cookieRole.Value}");
            bool moderRules = (cookieRole != null && (cookieRole.Value == "Admin" || cookieRole.Value == "Moder"));
            try
            {
                var article = Json(await _articleService.DeleteArticle(form, moderRules));
                _logger.LogInformation($"Article: \'{form.ArticleId}\' deleted");
                return Json(article);
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

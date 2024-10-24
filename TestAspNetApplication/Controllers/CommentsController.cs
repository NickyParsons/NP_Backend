using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using TestAspNetApplication.Data;
using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Services;

namespace TestAspNetApplication.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ILogger<CommentsController> _logger;
        private readonly PosgresDbContext _dbContext;
        private readonly CommentsService _commentsService;
        public CommentsController(
            ILogger<CommentsController> logger,
            PosgresDbContext dbContext,
            CommentsService commentsService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _commentsService = commentsService;
        }
        [HttpGet]
        [Route("/articles/{articleId}/comments")]
        public async Task<IActionResult> GetCommentsForArticle([FromRoute]Guid articleId)
        {
            return Json(await _commentsService.GetCommentsAtArticle(articleId));
        }
        [Authorize]
        [HttpPost]
        [Route("/articles/{articleId}/comments")]
        public async Task<IActionResult> PostCommentForArticle([FromRoute]Guid articleId, [FromForm]CreateCommentRequest form)
        {
            var cookieId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (form.AuthorId != cookieId)
            {
                _logger.LogDebug("User ID from cookie and user ID from request doesn't match");
                return BadRequest("Something wrong with AuthorID");
            }
            try
            {
                return Json(await _commentsService.AddComment(articleId, form));
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("/comments/{commentId}")]
        public async Task<IActionResult> EditComment([FromRoute] Guid commentId, [FromForm] CreateCommentRequest form)
        {
            var cookieId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (form.AuthorId != cookieId)
            {
                _logger.LogDebug("User ID from cookie and user ID from request doesn't match");
                return BadRequest("Something wrong with AuthorID");
            }
            var cookieRole = HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType);
            _logger.LogDebug($"Cookie role: {cookieRole}");
            bool adminRules = (cookieRole != null && cookieRole.Value == "Admin");
            try
            {
                return Json(await _commentsService.EditComment(commentId ,form, adminRules));
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

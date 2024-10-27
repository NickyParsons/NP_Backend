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
        private readonly CommentsService _commentsService;
        public CommentsController(
            ILogger<CommentsController> logger,
            CommentsService commentsService)
        {
            _logger = logger;
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
        public async Task<IActionResult> PostCommentForArticle(CreateCommentRequest form)
        {
            var cookieId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (form.AuthorId != cookieId)
            {
                _logger.LogDebug("User ID from cookie and user ID from request doesn't match");
                return BadRequest("Something wrong with AuthorID");
            }
            try
            {
                return Json(await _commentsService.AddComment(form));
            }
            catch (BadHttpRequestException e)
            {
                _logger.LogDebug(e.Message);
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("/comments/{commentId}/edit")]
        public async Task<IActionResult> EditComment(EditCommentRequest form)
        {
            var cookieId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (form.AuthorId != cookieId)
            {
                _logger.LogDebug("User ID from cookie and user ID from request doesn't match");
                return BadRequest("Something wrong with AuthorID");
            }
            var cookieRole = HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType);
            _logger.LogDebug($"Cookie role: {cookieRole}");
            bool adminRules = (cookieRole != null && (cookieRole.Value == "Admin" || cookieRole.Value == "Moder"));
            try
            {
                return Json(await _commentsService.EditComment(form, adminRules));
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [Route("/comments/{commentId}/delete")]
        public async Task<IActionResult> DeleteComment(DeleteCommentRequest form)
        {
            var cookieId = Guid.Parse(HttpContext.User.Claims.First(c => c.Type == "id").Value);
            if (form.AuthorId != cookieId)
            {
                _logger.LogDebug("User ID from cookie and user ID from request doesn't match");
                return BadRequest("Something wrong with AuthorID");
            }
            var cookieRole = HttpContext.User.Claims.First(x => x.Type == ClaimsIdentity.DefaultRoleClaimType);
            _logger.LogDebug($"Cookie role: {cookieRole.Value}");
            bool adminRules = (cookieRole != null && (cookieRole.Value == "Admin" || cookieRole.Value == "Moder"));
            try
            {
                return Json(await _commentsService.DeleteComment(form, adminRules));
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

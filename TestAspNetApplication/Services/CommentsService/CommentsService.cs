using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using Microsoft.EntityFrameworkCore;

namespace TestAspNetApplication.Services
{
    public class CommentsService
    {
        private readonly ILogger<CommentsService> _logger;
        private readonly PosgresDbContext _dbContext;
        public CommentsService(
            ILogger<CommentsService> logger,
            PosgresDbContext dbContext) 
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<Comment>> GetCommentsAtArticle(Guid articleId)
        {
            return await _dbContext.Comments.AsNoTracking().Where(c => c.ArticleId == articleId).ToListAsync();
        }
        public async Task<Comment> AddComment(Guid articleId, CreateCommentRequest form)
        {
            Comment comment = new Comment { 
                ArticleId = articleId, 
                AuthorId = form.AuthorId,
                Text = form.Text,
                CreatedAt = DateTime.UtcNow
            };
            await _dbContext.Comments.AddAsync(comment);
            _dbContext.SaveChanges();
            _logger.LogDebug($"Comment \'{form.Text}\' added to \'{articleId}\'");
            return comment;
        }
        public async Task<Comment> EditComment(Guid commentId, CreateCommentRequest form, bool adminRules)
        {
            Comment? dbComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (dbComment == null) 
            {
                _logger.LogDebug("Comment with this id not found");
                throw new BadHttpRequestException($"Comment with id {commentId} not found");
            }
            if (!(dbComment.AuthorId == form.AuthorId || adminRules))
            {
                _logger.LogDebug($"You have no permissions to edit comment {commentId}. Admin rules: {adminRules}. Author ID match: {dbComment.AuthorId == form.AuthorId}");
                _logger.LogDebug($"Author ID from form: {form.AuthorId}");
                _logger.LogDebug($"Author ID from comment in DB: {dbComment.AuthorId}");
                throw new BadHttpRequestException($"You have no permissions to edit this comment");
            }
            dbComment.Text = form.Text;
            dbComment.UpdatedAt = DateTime.UtcNow;
            _dbContext.SaveChanges();
            return dbComment;
        }
        public async Task<Comment> DeleteComment(Guid commentId, Guid authorId, bool adminRules)
        {
            Comment? dbComment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if (dbComment == null)
            {
                _logger.LogDebug("Comment with this id not found");
                throw new BadHttpRequestException($"Comment with id {commentId} not found");
            }
            if (!(dbComment.AuthorId == authorId || adminRules))
            {
                _logger.LogDebug($"You have no permissions to delete comment {commentId}. Admin rules: {adminRules}. Author ID match: {dbComment.AuthorId == authorId}");
                _logger.LogDebug($"Author ID from form: {authorId}");
                _logger.LogDebug($"Author ID from comment in DB: {dbComment.AuthorId}");
                throw new BadHttpRequestException($"You have no permissions to delete this comment");
            }
            _dbContext.Comments.Remove(dbComment);
            _dbContext.SaveChanges();
            return dbComment;
        }
    }
}
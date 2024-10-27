using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using Microsoft.EntityFrameworkCore;

namespace TestAspNetApplication.Services
{
    public class ArticleService
    {
        private readonly PosgresDbContext _dbContext;
        private readonly FileService _fileService;
        private readonly ILogger<ArticleService> _logger;
        public ArticleService(
            PosgresDbContext dbContext, 
            FileService fileService, 
            ILogger<ArticleService> logger) 
        {
            _dbContext = dbContext;
            _fileService = fileService;
            _logger = logger;
        }
        public async Task<Article> CreateArticle(CreateArticleRequest form, IFormFile? file)
        {
            Article article = new Article();
            article.Id = Guid.NewGuid();
            article.Name = form.Name;
            article.Description = form.Description;
            article.Text = form.Text;
            article.AuthorId = form.AuthorId;
            var now = DateTimeOffset.UtcNow;
            article.CreatedAt = now;
            article.UpdatedAt = now;
            if (file != null) article.ImageUrl = await _fileService.UploadFormFile(file, "articles", article.Id);
            _logger.LogDebug($"Trying add article: \'{article.Name}\'");
            await _dbContext.Articles.AddAsync(article);
            _dbContext.SaveChanges();
            return article;
        }
        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            return await _dbContext.Articles.ToListAsync();
        }
        public async Task<Article> EditArticle(EditArticleRequest form, bool moderRules, IFormFile? file)
        {
            Article? dbArticle = await _dbContext.Articles.FirstOrDefaultAsync(c => c.Id == form.ArticleId);
            if (dbArticle == null)
            {
                _logger.LogDebug("Article with this id not found");
                throw new BadHttpRequestException($"Article with id {form.ArticleId} not found");
            }
            if (!(dbArticle.AuthorId == form.AuthorId || moderRules))
            {
                _logger.LogDebug($"You have no permissions to edit article {form.ArticleId}. Admin rules: {moderRules}. Author ID match: {dbArticle.AuthorId == form.AuthorId}");
                _logger.LogDebug($"Author ID from form: {form.AuthorId}");
                _logger.LogDebug($"Author ID from comment in DB: {dbArticle.AuthorId}");
                throw new BadHttpRequestException($"You have no permissions to edit this article");
            }
            if (form.Name != null) dbArticle.Name = form.Name;
            if (form.Description != null) dbArticle.Description = form.Description;
            if (form.Text != null) dbArticle.Text = form.Text;
            if (file != null) dbArticle.ImageUrl = await _fileService.UploadFormFile(file, "articles", dbArticle.Id);
            dbArticle.UpdatedAt = DateTime.UtcNow;
            _dbContext.SaveChanges();
            return dbArticle;
        }
        public async Task<Article> DeleteArticle(DeleteArticleRequest form, bool moderRules)
        {
            Article? dbArticle = await _dbContext.Articles.FirstOrDefaultAsync(c => c.Id == form.ArticleId);
            if (dbArticle == null)
            {
                _logger.LogDebug("Article with this id not found");
                throw new BadHttpRequestException($"Article with id {form.ArticleId} not found");
            }
            if (!(dbArticle.AuthorId == form.AuthorId || moderRules))
            {
                _logger.LogDebug($"You have no permissions to delete article {form.ArticleId}. Admin rules: {moderRules}. Author ID match: {dbArticle.AuthorId == form.AuthorId}");
                _logger.LogDebug($"Author ID from form: {form.AuthorId}");
                _logger.LogDebug($"Author ID from comment in DB: {dbArticle.AuthorId}");
                throw new BadHttpRequestException($"You have no permissions to delete this article");
            }
            _dbContext.Articles.Remove(dbArticle);
            _dbContext.SaveChanges();
            return dbArticle;
        }
    }
}
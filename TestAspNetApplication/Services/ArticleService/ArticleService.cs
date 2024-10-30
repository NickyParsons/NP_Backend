using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json.Serialization;

namespace TestAspNetApplication.Services
{
    public class ArticleService
    {
        private readonly PosgresDbContext _dbContext;
        private readonly FileService _fileService;
        private readonly ILogger<ArticleService> _logger;
        private readonly IDistributedCache _cache;
        public ArticleService(
            PosgresDbContext dbContext, 
            FileService fileService, 
            ILogger<ArticleService> logger,
            IDistributedCache distributedCache) 
        {
            _dbContext = dbContext;
            _fileService = fileService;
            _logger = logger;
            _cache = distributedCache;
        }
        public async Task<Article> GetArticleById(Guid articleId)
        {
            var dbArticle = await _dbContext.Articles.FirstOrDefaultAsync(u=>u.Id == articleId);
            if (dbArticle == null)
            {
                _logger.LogDebug($"Article: \'{articleId}\' not found");
                throw new BadHttpRequestException("Article: '{articleId}' not found");
            }
            return dbArticle;
        }
        //public async Task<IEnumerable<Article>> GetAllArticles()
        //{
        //    return await _dbContext.Articles.ToListAsync();
        //}
        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            List<Article>? articles = null;
            try
            {
                string? articleString = await _cache.GetStringAsync($"all_articles".ToString());
                if (articleString != null)
                    articles = JsonSerializer.Deserialize<List<Article>>(articleString);
            }
            catch (Exception)
            {
                _logger.LogWarning("Redis error");
            }
            if (articles != null)
            {
                _logger.LogDebug($"Articles from cache");
            }
            else
            {
                _logger.LogDebug($"Articles not found in cache");
                articles = await _dbContext.Articles.AsNoTracking().ToListAsync();
                if (articles != null)
                {
                    UpdateArticlesInCacheAsync();
                }
            }
            if (articles == null) return new List<Article>();
            return articles!;
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
            if (file != null) article.ImageUrl = await _fileService.UploadFormFile(file, "articles", article.Id);
            _logger.LogDebug($"Trying add article: \'{article.Name}\'");
            await _dbContext.Articles.AddAsync(article);
            _dbContext.SaveChanges();
            UpdateArticlesInCacheAsync();
            return article;
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
            UpdateArticlesInCacheAsync();
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
            UpdateArticlesInCacheAsync();
            return dbArticle;
        }
        public async Task UpdateArticlesInCacheAsync()
        {
            List<Article> articles = await _dbContext.Articles.AsNoTracking().ToListAsync();
            string? articlesString = JsonSerializer.Serialize<List<Article>>(articles, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            try
            {
                await _cache.SetStringAsync($"all_articles", articlesString, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }
            catch (Exception)
            {
                _logger.LogWarning("Redis error");
            }
            _logger.LogDebug($"All articles cache updated!");
        }
    }
}
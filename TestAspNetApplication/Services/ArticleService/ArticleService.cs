using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;

namespace TestAspNetApplication.Services
{
    public class ArticleService
    {
        private readonly ArticleRepository _articleRepository;
        private readonly FileService _fileService;
        private readonly ILogger<ArticleService> _logger;
        public ArticleService(ArticleRepository articleRepository, FileService fileService, ILogger<ArticleService> logger) 
        {
            _articleRepository = articleRepository;
            _fileService = fileService;
            _logger = logger;
        }
        public async Task CreateArticle(CreateArticleRequest form, IFormFile? file)
        {
            Article article = new Article();
            article.Id = form.Id!.Value;
            article.Name = form.Name;
            article.Description = form.Description;
            article.Text = form.Text;
            article.AuthorId = form.AuthorId!.Value;
            var now = DateTimeOffset.UtcNow;
            article.CreatedAt = now;
            article.UpdatedAt = now;
            if (file != null)
            {
                article.ImageUrl = await _fileService.UploadFormFile(file, "articles", (Guid)form.Id);
            }
            _logger.LogDebug($"Trying add article: \'{article.Name}\'");
            await _articleRepository.CreateArticle(article);
            _logger.LogInformation($"Article: \'{article.Name}\' created");
        }
        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            return await _articleRepository.GetAllArticles();
        }
    }
}
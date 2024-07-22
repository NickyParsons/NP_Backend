using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;

namespace TestAspNetApplication.Services
{
    public class ArticleService
    {
        //private readonly IUserRepository _userRepository;
        private readonly ArticleRepository _articleRepository;
        private readonly ILogger<ArticleService> _logger;
        public ArticleService(ArticleRepository articleRepository, ILogger<ArticleService> logger) 
        {
            _articleRepository = articleRepository;
            //_userRepository = userRepository;
            _logger = logger;
        }
        public async Task CreateArticle(CreateArticleRequest form, IFormFile file)
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
            string relativeDir = $"content/articles/{form.Id}";
            string uploadDirectory = Path.Combine($"{Directory.GetCurrentDirectory()}\\wwwroot", relativeDir);
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }
            string relativePath = $"{relativeDir}/{file.FileName}";
            string uploadPath = Path.Combine(uploadDirectory, file.FileName);
            using (FileStream fileStream = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream);
            }
            article.ImageUrl = relativePath;
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
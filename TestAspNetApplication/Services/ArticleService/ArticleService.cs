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
        public async Task CreateArticle(CreateArticleRequest form)
        {
            Article article = new Article();
            article.Id = form.Id!.Value;
            article.Name = form.Name;
            article.Description = form.Description;
            article.Text = form.Text;
            article.ImageUrl = form.ImageUrl;
            article.AuthorId = form.AuthorId!.Value;
            var now = DateTimeOffset.UtcNow;
            article.CreatedAt = now;
            article.UpdatedAt = now;
            _logger.LogDebug($"Trying add article: \'{article.Name}\'");
            await _articleRepository.CreateArticle(article);
        }
    }
}
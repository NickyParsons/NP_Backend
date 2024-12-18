﻿using Microsoft.EntityFrameworkCore;
using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Data
{
    public class ArticleRepository
    {
        private PosgresDbContext _dbContext;
        private ILogger<ArticleRepository> _logger;
        public ArticleRepository(ILogger<ArticleRepository> logger, PosgresDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<Article> CreateArticle(Article newArticle)
        {
            await _dbContext.Articles.AddAsync(newArticle);
            await _dbContext.SaveChangesAsync();
            return newArticle;
        }
        public async Task<Article?> DeleteArticle(Guid id)
        {
            Article? dbArticle = await _dbContext.Articles.FirstOrDefaultAsync(x => x.Id == id);
            if(dbArticle != null)
            {
                _dbContext.Articles.Remove(dbArticle);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Article with id \'{id}\' not found");
            }
            return dbArticle;
        }
        public async Task<Article?> EditArticle(Article editedArticle)
        {
            Article? dbArticle = await _dbContext.Articles.FirstOrDefaultAsync(x => x.Id == editedArticle.Id);
            if (dbArticle != null)
            {
                dbArticle.Name = editedArticle.Name;
                dbArticle.Description = editedArticle.Description;
                dbArticle.Text = editedArticle.Text;
                dbArticle.ImageUrl = editedArticle.ImageUrl;
                dbArticle.UpdatedAt = editedArticle.UpdatedAt;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Article with id \'{editedArticle.Id}\' not found");
            }
            return dbArticle;
        }

        public async Task<IEnumerable<Article>> GetAllArticles()
        {
            return await _dbContext.Articles.AsNoTracking().ToListAsync();
        }
        public async Task<Article?> GetArticleById(Guid id)
        {
            Article? dbArticle = await _dbContext.Articles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if(dbArticle == null)
            {
                _logger.LogWarning($"Article with id \'{id}\' not found");
            }
            return dbArticle;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using TestAspNetApplication.FileLogger;
using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Data
{
    public class PersonRepository : IPersonRepository
    {
        private PosgresDbContext _dbContext;
        private ILogger<PersonRepository> _logger;
        public PersonRepository(PosgresDbContext dbContext, ILogger<PersonRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<Person> CreateAsync(Person entity)
        {
            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<Person?> DeleteAsync(int id)
        {
            Person? user = await _dbContext.Persons.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found");
            }
            else
            {
                _dbContext.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
            return user;
        }
        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            var users = await _dbContext.Persons.ToListAsync();
            return users;
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            Person? user = await _dbContext.Persons.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found");
            }
            return user;
        }

        public async Task<Person?> UpdateAsync(Person entity)
        {
            Person? user = await _dbContext.Persons.FindAsync(entity.Id);
            if (user != null)
            {
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.PhotoPath = entity.PhotoPath;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"User with ID {entity.Id} not found");
            }
            return user;
        }
    }
}

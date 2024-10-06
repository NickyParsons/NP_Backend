using Microsoft.EntityFrameworkCore;
using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Data
{
    public class UserRepository
    {
        private PosgresDbContext _dbContext;
        private ILogger<UserRepository> _logger;
        public UserRepository(ILogger<UserRepository> logger, PosgresDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<User> CreateUser(User newUser)
        {
            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
            _logger.LogDebug($"User \'{newUser.Email}\' created");
            return newUser;
        }
        public async Task<User?> DeleteUser(Guid id)
        {
            User? dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(dbUser != null)
            {
                _dbContext.Users.Remove(dbUser);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"User with id \'{id}\' not found");
            }
            return dbUser;
        }
        public async Task<User?> EditUser(User editedUser)
        {
            User? dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == editedUser.Id);
            if (dbUser != null)
            {
                dbUser.FirstName = editedUser.FirstName;
                dbUser.LastName = editedUser.LastName;
                dbUser.Email = editedUser.Email;
                dbUser.HashedPassword = editedUser.HashedPassword;
                dbUser.ImageUrl = editedUser.ImageUrl;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"User with id \'{editedUser.Id}\' not found");
            }
            return dbUser;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _dbContext.Users.Include(u => u.Role).ToListAsync();
        }
        public async Task<User?> GetUserByEmail(string email)
        {
            User? dbUser = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (dbUser == null)
            {
                _logger.LogWarning($"User with email \'{email}\' not found");
            }
            return dbUser;
        }
        public async Task<User?> GetUserById(Guid id, bool isIncluded)
        {
            User? dbUser;
            if (isIncluded)
            {
                dbUser = await _dbContext.Users.Include(u => u.Role).Include(u => u.Articles).FirstOrDefaultAsync(x => x.Id == id);
            }
            else 
            {
                dbUser = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            }
            if(dbUser == null)
            {
                _logger.LogWarning($"User with id \'{id}\' not found");
            }
            return dbUser;
        }
        public async Task<User?> GetUserByVerifyEmailToken(string token)
        {
            User? dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.VerificationToken == token);
            return dbUser;
        }
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.ConstrainedExecution;
using StackExchange.Redis;

namespace TestAspNetApplication.Services
{
    public class ProfileService
    {
        private readonly UserRepository _userRepository;
        private readonly FileService _fileService;
        private readonly ILogger<ProfileService> _logger;
        private readonly PosgresDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IDistributedCache _cache;
        public ProfileService(FileService fileService, 
            ILogger<ProfileService> logger, 
            UserRepository userRepository,
            PosgresDbContext dbContext,
            IPasswordHasher passwordHasher,
            IDistributedCache distributedCache) 
        {
            _fileService = fileService;
            _userRepository = userRepository;
            _logger = logger;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _cache = distributedCache;
        }
        public async Task<User> GetUserWithRoleByIdAsync(Guid id)
        {
            User? dbUser = null;
            try
            {
                string? userString = await _cache.GetStringAsync($"user_{id}".ToString());
                if (userString != null)
                    dbUser = JsonSerializer.Deserialize<User>(userString);
            }
            catch (Exception)
            {
                _logger.LogWarning("Redis error");
            }
            if (dbUser != null)
            {
                _logger.LogDebug($"User {dbUser.Id} from cache");
            }
            else
            {
                _logger.LogDebug($"User {id} not found in cache");
                dbUser = await _dbContext.Users.AsNoTracking().Include(u => u.Role).FirstOrDefaultAsync(x => x.Id == id);
                if (dbUser != null)
                {
                    UpdateProfileInCacheAsync(id);
                }
            }
            if (dbUser == null) throw new BadHttpRequestException("User not found");
            return dbUser!;
        }
        public async Task<User> EditProfile(EditUserRequest form, IFormFile? file)
        {
            User? dbUser = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Id == form.UserId);
            if (dbUser == null) throw new BadHttpRequestException($"User {form.UserId} not found");
            if (form.isPasswordChanging)
            {
                if (form.newPassword == null) throw new BadHttpRequestException("New password is empty");
                if (form.newPassword.Length < 6 || form.newPassword.Length <=0) throw new BadHttpRequestException("6 symbols minimum");
                if (form.newPassword != null && form.oldPassword == null) throw new BadHttpRequestException("Current password is empty");
                if (!_passwordHasher.Verify(dbUser.HashedPassword, form.oldPassword!)) throw new BadHttpRequestException("Current password incorrect");
                dbUser.HashedPassword = _passwordHasher.GenerateHash(form.newPassword!);
            }
            if (form.Firstname != null) dbUser.FirstName = form.Firstname;
            if (form.Lastname != null) dbUser.LastName = form.Lastname;
            if (file != null) dbUser.ImageUrl = await _fileService.UploadFormFile(file, "profiles", (Guid)dbUser.Id);
            _dbContext.SaveChanges();
            UpdateProfileInCacheAsync(form.UserId);
            return dbUser;
        }
        public async Task UpdateProfileInCacheAsync(Guid id)
        {
            User? dbUser = await _dbContext.Users.AsNoTracking().Include(u => u.Role).FirstOrDefaultAsync(x => x.Id == id);
            if (dbUser != null)
            {
                string? userString = JsonSerializer.Serialize<User>(dbUser, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
                try
                {
                    await _cache.SetStringAsync($"user_{id}", userString, new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(30)
                    });
                }
                catch (Exception)
                {
                    _logger.LogWarning("Redis error");
                }
                _logger.LogDebug($"User cache {dbUser.Id} updated!");
            }
        }
    }
}
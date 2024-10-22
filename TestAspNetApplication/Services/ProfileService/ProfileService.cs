using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using Microsoft.EntityFrameworkCore;

namespace TestAspNetApplication.Services
{
    public class ProfileService
    {
        private readonly UserRepository _userRepository;
        private readonly FileService _fileService;
        private readonly ILogger<ProfileService> _logger;
        private readonly PosgresDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        public ProfileService(FileService fileService, 
            ILogger<ProfileService> logger, 
            UserRepository userRepository,
            PosgresDbContext dbContext,
            IPasswordHasher passwordHasher) 
        {
            _fileService = fileService;
            _userRepository = userRepository;
            _logger = logger;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }
        public async Task<User?> GetProfileData(Guid id)
        {
            User? dbUser = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Id == id);
            if (dbUser == null) throw new BadHttpRequestException("User not found");
            return dbUser;
        }
        public async Task<User> EditProfile(EditUserRequest form, IFormFile? file)
        {
            User? dbUser = await _dbContext.Users.Include(u => u.Role).Include(u => u.Articles).FirstOrDefaultAsync(x => x.Id == form.Id);
            if (dbUser == null) throw new BadHttpRequestException($"User {form.Id} not found");
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
            return dbUser;
        }
    }
}
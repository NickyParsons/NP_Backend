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
        PosgresDbContext _dbContext;
        public ProfileService(FileService fileService, 
            ILogger<ProfileService> logger, 
            UserRepository userRepository,
            PosgresDbContext dbContext) 
        {
            _fileService = fileService;
            _userRepository = userRepository;
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<User?> GetProfileData(Guid id)
        {
            User? dbUser = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Id == id);
            if (dbUser == null) throw new BadHttpRequestException("User not found");
            return dbUser;
        }
        public async Task<User> EditProfile(EditUserRequest form, IFormFile? file)
        {
            if (form.isPasswordChanging)
            {
                _logger.LogInformation($"Нужно поменять пароль");
            }
            else
            {
                _logger.LogInformation($"Менять пароль не нужно");
            }


            var user = await _userRepository.GetUserById(form.Id, true);
            if (user != null)
            {
                if (form.Firstname != null)
                {
                    user.FirstName = form.Firstname;
                }
                if (form.Lastname != null)
                {
                    user.LastName = form.Lastname;
                }
                if (file != null)
                {
                    user.ImageUrl = await _fileService.UploadFormFile(file, "profiles", (Guid)user.Id);
                }
                if (await _userRepository.EditUser(user) == null)
                {
                    throw new Exception("Something gone wrong while editing user");
                }
            }
            return user!;
        }
    }
}
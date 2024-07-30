using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;

namespace TestAspNetApplication.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly FileService _fileService;
        private readonly ILogger<UserService> _logger;
        public UserService(FileService fileService, ILogger<UserService> logger, IUserRepository userRepository) 
        {
            _fileService = fileService;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<User> EditUser(EditUserRequest form, IFormFile? file)
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
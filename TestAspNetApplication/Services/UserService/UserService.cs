using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;

namespace TestAspNetApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _hasher;
        private readonly IUserRepository _userRepo;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRoleRepository _roleRepo;
        private readonly ILogger<UserService> _logger;
        public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository, IJwtProvider jwtProvider, IRoleRepository roleRepository, ILogger<UserService> logger) 
        { 
            _hasher = passwordHasher;
            _userRepo = userRepository;
            _jwtProvider = jwtProvider;
            _roleRepo = roleRepository;
            _logger = logger;
        }
        public async Task<string> Login(string username, string password)
        {
            string token = string.Empty;
            var user = await _userRepo.GetUserByEmail(username);
            if (user != null)
            {
                if (_hasher.Verify(user.HashedPassword, password))
                {
                    token = _jwtProvider.GenerateToken(user);
                }
            }
            return token;
        }
        public async Task Register(string email, string password, string? firstname, string? lastname)
        {
            User user = new User();
            user.Email = email;
            user.HashedPassword = _hasher.GenerateHash(password);
            user.FirstName = firstname;
            user.LastName = lastname;
            Role? userRole = await _roleRepo.GetRoleByName("user");
            if (userRole == null)
            {
                _logger.LogDebug("Пытаюсь создать роль Юзера");
                user.Role = await _roleRepo.CreateRole("user");
                _logger.LogDebug("Роль создана");
            }
            else
            {
                _logger.LogDebug("Роль найдена");
                user.Role = userRole!;
            }
            _logger.LogDebug($"Пытаюсь создать Юзера c ID: {user.Id} и RoleID {user.Role.Id}");
            await _userRepo.CreateUser(user);
        }
    }
}
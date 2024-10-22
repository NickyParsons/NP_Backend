using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using Microsoft.EntityFrameworkCore;

namespace TestAspNetApplication.Services
{
    public class AuthService
    {
        private readonly PosgresDbContext _dbContext;
        private readonly IPasswordHasher _hasher;
        private readonly UserRepository _userRepo;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRoleRepository _roleRepo;
        private readonly ILogger<AuthService> _logger;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IEmailSender _emailSender;
        public AuthService(
            PosgresDbContext dbContext, 
            IPasswordHasher passwordHasher, 
            IJwtProvider jwtProvider, 
            UserRepository userRepository, 
            IRoleRepository roleRepository, 
            ILogger<AuthService> logger, 
            TokenGenerator tokenGenerator,
            IEmailSender emailSender) 
        { 
            _dbContext = dbContext;
            _hasher = passwordHasher;
            _userRepo = userRepository;
            _jwtProvider = jwtProvider;
            _roleRepo = roleRepository;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
            _emailSender = emailSender;
        }
        public async Task<string> Login(LoginUserRequest form)
        {
            var user = await _userRepo.GetUserByEmail(form.Email);
            if (user == null) throw new BadHttpRequestException($"User {form.Email} not found");
            if (!_hasher.Verify(user.HashedPassword, form.Password)) throw new BadHttpRequestException($"Password for {form.Email} incorrect");
            return _jwtProvider.GenerateToken(user);
        }
        public async Task Register(RegisterUserRequest form)
        {
            var dbUser = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == form.Email);
            if (dbUser != null) {
                throw new BadHttpRequestException("User with this email already exist");
            }
            User user = new User();
            user.Email = form.Email;
            user.HashedPassword = _hasher.GenerateHash(form.Password);
            user.FirstName = form.Firstname;
            user.LastName = form.Lastname;
            Role? userRole = await _roleRepo.GetRoleByName("user");
            if (userRole == null)
            {
                user.Role = await _roleRepo.CreateRole(new Role { Name = "User", Description = "Пользователь" });
            }
            else
            {
                user.Role = userRole!;
            }
            string token;
            User? dbUserByToken;
            do
            {
                token = _tokenGenerator.GenerateToken();
                dbUserByToken = await _dbContext.Users.FirstOrDefaultAsync(x => x.VerificationToken == token);
            }
            while (dbUserByToken != null);
            user.VerificationToken = token;
            await _dbContext.AddAsync(user);
            _dbContext.SaveChanges();
            _logger.LogDebug($"User \'{user.Email}\' created");
            await _emailSender.SendVerifyEmailTokenAsync(user.VerificationToken, user.Email);
        }
        public async Task VerifyEmail(string token)
        {
            User? dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.VerificationToken == token);
            if (dbUser == null) throw new BadHttpRequestException("Token not found");
            dbUser.VerifiedAt = DateTime.UtcNow;
            _dbContext.SaveChanges();
        }
        public async Task ChangeEmail(ChangeEmailRequest form)
        {
            User? dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == form.Email);
            if (dbUser == null) throw new BadHttpRequestException("User with this e-mail not found");
            User? dbUserWithNewEmail = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == form.NewEmail);
            if (dbUserWithNewEmail != null) throw new BadHttpRequestException("User with this e-mail already exist");
            dbUser.Email = form.NewEmail;
            dbUser.VerifiedAt = null;
            string token;
            User? dbUserByToken;
            do
            {
                token = _tokenGenerator.GenerateToken();
                dbUserByToken = await _dbContext.Users.FirstOrDefaultAsync(x => x.VerificationToken == token);
            }
            while (dbUserByToken != null);
            dbUser.VerificationToken = token;
            _dbContext.SaveChanges();
            await _emailSender.SendVerifyEmailTokenAsync(dbUser.VerificationToken, dbUser.Email);
        }
        public async Task ForgotPassword(string email)
        {
            User? dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (dbUser == null) throw new BadHttpRequestException("User with this email not found");
            string token;
            User? dbUserByToken;
            do
            {
                token = _tokenGenerator.GenerateToken();
                dbUserByToken = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.PasswordResetToken == token);
            } while (dbUserByToken != null);
            dbUser.PasswordResetToken = token;
            dbUser.ResetTokenExpires = DateTime.UtcNow.AddHours(1);
            _dbContext.SaveChanges();
            await _emailSender.SendResetPasswordTokenAsync(dbUser.PasswordResetToken, dbUser.Email);
        }
        public async Task ResetPassword(ResetPasswordRequest form)
        {
            User? dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.PasswordResetToken == form.Token);
            if (dbUser == null) throw new BadHttpRequestException("Token not found");
            if(DateTime.UtcNow > dbUser.ResetTokenExpires) throw new BadHttpRequestException("Token expired");
            dbUser.HashedPassword = _hasher.GenerateHash(form.Password);
            _dbContext.SaveChanges();
        }
    }
}
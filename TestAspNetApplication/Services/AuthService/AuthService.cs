using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;
using TestAspNetApplication.Enums;
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
        public async Task<LoginUserResponse> Login(LoginUserRequest form)
        {
            var dbUser = await _dbContext.Users.Include(u=>u.Role).FirstOrDefaultAsync(u => u.Email == form.Email);
            if (dbUser == null)
            {
                _logger.LogDebug($"User {form.Email} not found");
                throw new BadHttpRequestException($"User {form.Email} not found"); 
            }
            if (!_hasher.Verify(dbUser.HashedPassword, form.Password)) throw new BadHttpRequestException($"Password for {form.Email} incorrect");
            string accessToken = _jwtProvider.GenerateToken(dbUser);
            dbUser.RefreshToken = await GenerateToken(GeneratedTokenType.RefreshToken);
            dbUser.RefreshTokenExpires = DateTime.UtcNow.AddDays(7);
            _dbContext.SaveChanges();
            return new LoginUserResponse { AccessToken = accessToken, RefreshToken = dbUser.RefreshToken };
        }
        public async Task<LoginUserResponse> RefreshAccessToken(string accessToken, string refreshToken)
        {
            var principal = _jwtProvider.GetClaimsPrincipal(accessToken);
            if (principal?.Identity?.Name is null)
            {
                _logger.LogDebug("Can't get e-mail from access token");
                throw new BadHttpRequestException("Can't get e-mail from access token");
            }
            string tokenEmail = principal.Identity.Name;
            User? dbUser = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == tokenEmail);
            if (dbUser == null)
            {
                _logger.LogDebug($"User {tokenEmail} not found");
                throw new BadHttpRequestException($"User {tokenEmail} not found");
            }
            if (refreshToken != dbUser.RefreshToken || DateTime.UtcNow > dbUser.RefreshTokenExpires)
            {
                _logger.LogDebug($"Invalid refresh token");
                throw new BadHttpRequestException($"Invalid refresh token");
            }
            string newAccessToken = _jwtProvider.GenerateToken(dbUser);
            dbUser.RefreshToken = await GenerateToken(GeneratedTokenType.RefreshToken);
            dbUser.RefreshTokenExpires = DateTime.UtcNow.AddHours(24);
            _dbContext.SaveChanges();
            _logger.LogDebug($"Access token for {dbUser.Email} updated!");
            return new LoginUserResponse { AccessToken = newAccessToken, RefreshToken = dbUser.RefreshToken };
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
            user.VerificationToken = await GenerateToken(GeneratedTokenType.VerifyEmail);
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
            dbUser.VerificationToken = await GenerateToken(GeneratedTokenType.VerifyEmail);
            _dbContext.SaveChanges();
            await _emailSender.SendVerifyEmailTokenAsync(dbUser.VerificationToken, dbUser.Email);
        }
        public async Task ForgotPassword(string email)
        {
            User? dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (dbUser == null) throw new BadHttpRequestException("User with this email not found");
            dbUser.PasswordResetToken = await GenerateToken(GeneratedTokenType.ForgotPassword);
            dbUser.ResetTokenExpires = DateTime.UtcNow.AddHours(12);
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
        private async Task<string> GenerateToken(GeneratedTokenType tokenType)
        {
            string token;
            User? dbUserByToken;
            do
            {
                token = _tokenGenerator.GenerateToken();
                switch (tokenType)
                {
                    case GeneratedTokenType.ForgotPassword:
                        dbUserByToken = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.PasswordResetToken == token);
                        break;
                    case GeneratedTokenType.VerifyEmail:
                        dbUserByToken = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.VerificationToken == token);
                        break;
                    case GeneratedTokenType.RefreshToken:
                        dbUserByToken = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.RefreshToken == token);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                
            } while (dbUserByToken != null);
            return token;
        }
    }
}
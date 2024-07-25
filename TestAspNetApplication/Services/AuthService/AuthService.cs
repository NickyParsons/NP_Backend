﻿using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;

using TestAspNetApplication.Data.Entities;
using TestAspNetApplication.Data;
using TestAspNetApplication.DTO;

namespace TestAspNetApplication.Services
{
    public class AuthService
    {
        private readonly IPasswordHasher _hasher;
        private readonly IUserRepository _userRepo;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRoleRepository _roleRepo;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IPasswordHasher passwordHasher, IJwtProvider jwtProvider, IUserRepository userRepository, IRoleRepository roleRepository, ILogger<AuthService> logger) 
        { 
            _hasher = passwordHasher;
            _userRepo = userRepository;
            _jwtProvider = jwtProvider;
            _roleRepo = roleRepository;
            _logger = logger;
        }
        public async Task<string?> Login(LoginUserRequest form)
        {
            var user = await _userRepo.GetUserByEmail(form.Email);
            if (user != null)
            {
                if (_hasher.Verify(user.HashedPassword, form.Password))
                {
                    return _jwtProvider.GenerateToken(user);
                }
            }
            return null;
        }
        public async Task Register(RegisterUserRequest form)
        {
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
            await _userRepo.CreateUser(user);
        }
    }
}
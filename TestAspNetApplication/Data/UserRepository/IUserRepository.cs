﻿using TestAspNetApplication.Data.Entities;

namespace TestAspNetApplication.Data
{
    public interface IUserRepository : IDisposable
    {
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User?> GetUserById(int id);
        public Task<User?> GetUserByEmail(string email);
        public Task<User> CreateUser(User newUser);
        public Task<User?> EditUser(User editedUser);
        public Task<User?> DeleteUser(int id);
    }
}

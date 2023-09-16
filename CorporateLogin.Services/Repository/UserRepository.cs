using CorporateLogin.Common.Interfaces;
using CorporateLogin.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CorporateLogin.Services.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _users;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _users = context.Users;
        }

        public User GetUserByName(string name)
        {
            return _users.FirstOrDefault(u => u.Username == name);
        }
        public bool CheckUserExistByName(string name)
        {
            return _users.Any(u => u.Username == name);
        }

        public User Update(User user)
        {
            if (user == null) return null;

            var newUser = _users.Update(user);
            _context.SaveChanges();
            return newUser.Entity;
        }

    }
}
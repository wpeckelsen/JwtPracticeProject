using JwtPracticeProject.Data;
using JwtPracticeProject.Models;

namespace JwtPracticeProject.Service
{


    public class UserService : IUserService
    {


        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

    }
}
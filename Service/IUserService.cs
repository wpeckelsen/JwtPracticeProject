using JwtPracticeProject.Models;

namespace JwtPracticeProject.Service
{

    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);

        Task<bool> doesUserExistByUsernameAsync(string username);

        Task<User> CreateUserAsync(string Username, string plainPassword);
        
        // Task<User> CreateUserAndRoleAsync(string username, string plainPassword, string role);

        string GenerateJwtToken(User user);

        Task<string?> Authenticate(string Username, string password);

    }
}
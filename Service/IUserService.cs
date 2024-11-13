using JwtPracticeProject.Models;

namespace JwtPracticeProject.Service{

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    
    Task<bool> doesUserExistByEmailAsync(string username);

    Task<User> CreateUserAsync(string Username, string plainPassword);
    
    string GenerateJwtToken(User user);
    
    Task<string?> Authenticate(string Username, string password);
        
    }
}
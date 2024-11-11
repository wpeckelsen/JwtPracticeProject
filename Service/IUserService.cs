using JwtPracticeProject.Models;

namespace JwtPracticeProject.Service{

public interface IUserService
{
    Task<User> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
}
}
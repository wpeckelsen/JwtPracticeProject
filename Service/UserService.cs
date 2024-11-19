using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using JwtPracticeProject.Data;
using JwtPracticeProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JwtPracticeProject.Service
{


    public class UserService : IUserService
    {


        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> doesUserExistByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user != null;
        }

        public async Task<User> CreateUserAsync(string username, string plainPassword)
        {
            var foundUser = await doesUserExistByUsernameAsync(username);
            if (foundUser)
            {
                throw new Exception("this email is already taken.");
            }
            else
            {
                User user = new User
                {
                    Username = username,
                    HashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword),                    
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
        }

       


        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Retrieve from configuration


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Username),              // Username claim
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID claim
            new Claim(ClaimTypes.Role, "user"),
            new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),  // Audience claim
            new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"])     // Issuer claim



                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async Task<string?> Authenticate(string username, string password)
        {
            // Find the user by username
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);


            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.HashedPassword))
            {
                return null;
            }

            string generatedToken = GenerateJwtToken(user);
            return generatedToken;
        }




    }
}
using Microsoft.EntityFrameworkCore;
using JwtPracticeProject.Models;


namespace JwtPracticeProject.Data
{
    public class ApplicationDbContext : DbContext
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
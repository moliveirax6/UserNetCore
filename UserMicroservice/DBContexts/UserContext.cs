using Microsoft.EntityFrameworkCore;
using UserMicroservice.Models;

namespace UserMicroservice.DBContexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Fulano Beta Teste",
                    UserName = "beta.tester",
                    Password = "321",
                    Email = "beta.tester@email.com",
                    Token = ""
                }
            );
        }
    }
}

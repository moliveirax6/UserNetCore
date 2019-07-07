using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserMicroservice.DBContexts;
using UserMicroservice.Models;

namespace UserMicroservice.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _dbContext;

        public UserRepository(UserContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteUser(int userId)
        {
            var user = _dbContext.Users.Find(userId);
            _dbContext.Users.Remove(user);
            Save();
        }

        public User GetUserById(int userId)
        {
            return _dbContext.Users.Find(userId);
        }

        public User GetUserByUserName(string userName)
        {
            try
            {
                return _dbContext.Users.SingleOrDefault(p => p.UserName == userName);
            }
            catch
            {
                return null;
            }
        }

        public List<User> GetUsers()
        {
            try
            {
                return _dbContext.Users.ToList();
            } 
            catch
            {
                return null;
            }
        }
        public void InsertUser(User user)
        {
            _dbContext.Add(user);
            Save();
        }

        public UserAuth Authentication(User user)
        {
            UserAuth result = new UserAuth();
            try
            {
                var comparator = _dbContext.Users.SingleOrDefault(p => p.UserName == user.UserName);
                if (comparator == null || comparator.Id < 1)
                {
                    result.Authenticated = false;
                    result.Error = "Usuário inválido !";
                }
                else if (comparator.Password != user.Password)
                {
                    result.Authenticated = false;
                    result.Error = "Senha inválida !";
                }
                else
                {
                    result.Name = comparator.Name;
                    result.Email = comparator.Email;
                    result.Id = comparator.Id;
                    result.Authenticated = true;
                    result.Token = GenerateJSONWebToken();
                }
            }
            catch (Exception e)
            {
                result.Authenticated = false;
                result.Error = e.Message;
            }
            return result;
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Program.Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(Program.Configuration["Jwt:Issuer"],
              Program.Configuration["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            Save();
        }
    }
}

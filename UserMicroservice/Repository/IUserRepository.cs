using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Models;

namespace UserMicroservice.Repository
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User GetUserById(int userId);
        User GetUserByUserName(string email);
        void InsertUser(User user);
        void DeleteUser(int userId);
        void UpdateUser(User user);
        UserAuth Login(User user);
        void Save();
    }
}

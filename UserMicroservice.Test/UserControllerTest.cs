using System;
using UserMicroservice.Controllers;
using UserMicroservice.Repository;
using UserMicroservice.Models;
using UserMicroservice.DBContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions.Ordering;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCaseOrderer("Xunit.Extensions.Ordering.TestCaseOrderer", "Xunit.Extensions.Ordering")]
[assembly: TestCollectionOrderer("Xunit.Extensions.Ordering.CollectionOrderer", "Xunit.Extensions.Ordering")]

namespace UserControllerTest.Test
{
    [Collection("Sequential")]
    public class UserControllerTest
    {
        UserRepository _userRepository;
        private User _user = new User();
        public UserControllerTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseMySQL("Server=localhost;DataBase=Users;Uid=root;");

            var context = new UserContext(optionsBuilder.Options);
            _userRepository = new UserRepository(context);

            _user.Name = "NAME TESTER 1";
            _user.UserName = "USER NAME TESTER 1";
            _user.Email = "tester.tester@email.com";
            _user.Password = "t2js2t";
        }       

        [Fact, Order(1)]
        public void CommunicationDb()
        {
            var resp = _userRepository.GetUsers();

            Assert.NotNull(resp);
        }

        [Fact, Order(2)]
        public void GetUsers()
        {
            var resp = _userRepository.GetUsers();

            Assert.NotNull(resp);
        }

        [Fact, Order(3)]
        public void InsertUser()
        {
            _userRepository.InsertUser(_user);
            var resp = _userRepository.GetUserByUserName(_user.UserName);
      
            Assert.NotNull(resp);
        }

        [Fact, Order(4)]
        public void UpdateUser()
        {
            _user = _userRepository.GetUserByUserName(_user.UserName);
            string name = "NAME TESTER 1";
            _user.Name = name;
            _userRepository.UpdateUser(_user);
            _user = _userRepository.GetUserById(_user.Id);           

            Assert.Equal(_user.Name, name);
        }

        [Fact, Order(5)]
        public void DeleteUser()
        {
            _user = _userRepository.GetUserByUserName(_user.UserName);
            _userRepository.DeleteUser(_user.Id);
            var resp = _userRepository.GetUserById(_user.Id);

            Assert.Null(resp);
        }
    }
}

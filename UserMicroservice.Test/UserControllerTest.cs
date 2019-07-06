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
    public class PersonControllerTest
    {
        UserRepository _userRepository;
        private User _user = new User();
        public PersonControllerTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserContext>();
            optionsBuilder.UseSqlServer("Server=localhost;DataBase=Devices;Uid=root;");

            var context = new UserContext(optionsBuilder.Options);
            _userRepository = new UserRepository(context);
            
            _user.Email = "XXXXXXXXXXX@XXXXXXXXXXX.XXX";
            _user.Name = "TESTE XXXXXXXXXXXXXXX";
            _user.Password = "XXXXXXXXXXXXXXXXX";
        }

        private void CleanTester()
        {
            var resp = _userRepository.GetUserByUserName(_user.UserName);
            while(resp != null)
            {
                _userRepository.DeleteUser(resp.Id);
            }
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
            CleanTester();
            _userRepository.InsertUser(_user);
            var resp = _userRepository.GetUserByUserName(_user.UserName);
      
            Assert.NotNull(resp);
        }

        [Fact, Order(4)]
        public void UpdateUser()
        {
            _user = _userRepository.GetUserByUserName(_user.UserName);
            string newName = "TESTE XXXXXXXXXXXXXXXYYYYYY";
            _user.Name = newName;
            _userRepository.UpdateUser(_user);
            var resp = _userRepository.GetUserById(_user.Id);           

            Assert.Equal(resp.Name, newName);
        }

        [Fact, Order(5)]
        public void LoginUser()
        {
            var resp = _userRepository.Login(_user);

            Assert.True(resp.Authenticated);
        }

        [Fact, Order(6)]
        public void DeletePerson()
        {
            _user = _userRepository.GetUserByUserName(_user.UserName);
            _userRepository.DeleteUser(_user.Id);
            var resp = _userRepository.GetUserById(_user.Id);

            Assert.Null(resp);
        }
    }
}

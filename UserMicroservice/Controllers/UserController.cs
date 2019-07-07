using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using UserMicroservice.Models;
using UserMicroservice.Repository;

namespace UserMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/User
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            { 
                var users = _userRepository.GetUsers();
                return new OkObjectResult(users);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET: api/User/0
        [HttpGet("{id}", Name = "Get")]
        [Authorize]
        public IActionResult Get(int id)
        {
            try
            {
                var users = _userRepository.GetUserById(id);
                return new OkObjectResult(users);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // POST: api/user
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] User user)
        {
            try
            {
                _userRepository.InsertUser(user);
                return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("authentication")]
        [AllowAnonymous]
        public UserAuth Authentication([FromBody] User user)
        { 
            return _userRepository.Authentication(user);
        }

        // PUT: api/User/0
        [HttpPut]
        [Authorize]
        public IActionResult Put(long id, [FromBody] User user)
        {
            try
            { 
                if (user != null)
                {
                   _userRepository.UpdateUser(user);
                   return new OkResult();
                }
                return new NoContentResult();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // DELETE: api/ApiWithActions/0
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                _userRepository.DeleteUser(id);
                return new OkResult();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
